using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Entity
{
	[AttributeUsage( AttributeTargets.Property )]
	public class LocalStorageAttribute : Attribute
	{}

	public class LocalStoragePropertyProcessor
	{
		static readonly MethodInfo ApplyMethod = typeof(LocalStoragePropertyProcessor).GetMethod( "Apply", DragonSparkBindingOptions.AllProperties );

		public static LocalStoragePropertyProcessor Instance
		{
			get { return InstanceField; }
		}	static readonly LocalStoragePropertyProcessor InstanceField = new LocalStoragePropertyProcessor();

		public void Process( DbContext context, DbModelBuilder modelBuilder, bool useConvention = true )
		{
			var types = context.GetDeclaredEntityTypes().Select( x => x.GetHierarchy( false ).Last() ).Distinct().SelectMany( x => x.Assembly.GetTypes().Where( y => x.Namespace == y.Namespace ) ).Distinct().ToArray();

			types.SelectMany( y => y.GetProperties( DragonSparkBindingOptions.AllProperties ).Where( z => z.IsDecoratedWith<LocalStorageAttribute>() || ( useConvention && FollowsConvention( z ) )  ) ).Apply( x =>
			{
				ApplyMethod.MakeGenericMethod( x.DeclaringType, x.PropertyType ).Invoke( this, new object[] { modelBuilder, x } );
			} );
		}

		static bool FollowsConvention( PropertyInfo propertyInfo )
		{
			var result = propertyInfo.Name.EndsWith( "Storage" ) && propertyInfo.DeclaringType.GetProperty( propertyInfo.Name.Replace( "Storage", string.Empty ) ).Transform( x => x.IsDecoratedWith<NotMappedAttribute>() );
			return result;
		}

		void Apply<TEntity, TProperty>( DbModelBuilder builder, PropertyInfo property ) where TEntity : class
		{
			var parameter = Expression.Parameter( typeof(TEntity), "x" );
			var expression = Expression.Property( parameter, property.Name );
			
			var result = Expression.Lambda<Func<TEntity, TProperty>>( expression, parameter );

			var configuration = builder.Entity<TEntity>();
			configuration.GetType().GetMethod( "Property", new[] { result.GetType() } ).NotNull( y => y.Invoke( configuration, new object[] { result } ) );
		}
	}

	public static class DbContextExtensions
	{
		static readonly MethodInfo 
			GetMethod = typeof(DbContextExtensions).GetMethod( "Get", new[] { typeof(DbContext), typeof(object), typeof(int) } );

		public static int Save( this DbContext target )
		{
			try
			{
				return target.SaveChanges();
			}
			catch ( DbEntityValidationException error )
			{
				error.EntityValidationErrors.Apply( x => Log.Warning( string.Format( "Entity Validation Error: {0}.{1}{2}", x.Entry.GetType(), System.Environment.NewLine, string.Join( System.Environment.NewLine, x.ValidationErrors.Select( y => y.ErrorMessage ).ToArray() ) ) ) );
				throw;
			}
		}

		public static TEntity ApplyChanges<TEntity>( this DbContext target, TEntity entity ) where TEntity : class
		{
			switch ( target.Entry( entity ).State )
			{
				case EntityState.Detached:
					target.Set<TEntity>().AddOrUpdate( entity );

					/*var type = entity.GetType();
					var properties = target.GetEntityProperties( type ).Select( x => x.Name ).ToArray();
				
					properties.Apply( x =>
					{
						var property = type.GetProperty( x );
						var raw = property.GetValue( entity );
						var items = property.GetCollectionType() != null ? raw.To<IEnumerable>().Cast<object>().ToArray() : new[] { raw };
						items.Apply( y => target.Set( y.GetType() ).Remove( y ) );
						target.SaveChanges();
					} );*/

					break;
			}
			return entity;
		}

		public static object Get( this DbContext target, object entity, Type entityType = null )
		{
			var result = GetMethod.MakeGenericMethod( entityType ?? entity.GetType() ).Invoke( null, new[] { target, entity, 1 } );
			return result;
		}

		public static TItem Get<TItem>( this DbContext target, object container, int levels = 1 ) where TItem : class
		{
			using ( target.Configured( x => x.AutoDetectChangesEnabled = false ) )
			{
				// using ( var tracer = new Tracer() )
				{
					var key = target.DetermineKey<TItem>( container );
					/*var builder = new EntityKeyExpressionBuilder<TItem>( key );
					var expression = builder.Create();*/
					/*var singleOrDefault = target.Set<TItem>().Local.SingleOrDefault( expression.Compile() );
					var includes = DetermineDefaultNavigationProperties( target, typeof(TItem) );
					var result = singleOrDefault ?? target.WithIncludes<TItem>().SingleOrDefault( expression );*/

					var current = target.Set<TItem>().Find( key.Values.ToArray() );
					// tracer.Mark( string.Format( "Get.Store.Find. Type: {0}", typeof(TItem).Name ) );
					
					var result = current.Transform( x => target.Include( x, levels ) );

					// tracer.Mark( string.Format( "Get.Store.Include. Type: {0}", typeof(TItem).Name ) );

					return result;
				}

			}
		}

		public static IQueryable<TItem> WithIncludes<TItem>( this DbContext target ) where TItem : class
		{
			return WithIncludes( target.Set<TItem>(), target );
		}

		public static IQueryable<TItem> WithIncludes<TItem>( this IQueryable<TItem> target ) where TItem : class
		{
			return WithIncludes( target, new QueryWrapper( target ) );
		}

		static IQueryable<TItem> WithIncludes<TItem>( IQueryable<TItem> target, IObjectContextAdapter adapter ) where TItem : class
		{
			var names = DetermineDefaultAssociationPaths( adapter, typeof(TItem) ).ToArray();
			var result = names.Aggregate( target, ( current, item ) => current.Include( item ) );
			return result;
		}

		class QueryWrapper : IObjectContextAdapter
		{
			readonly IQueryable query;

			public QueryWrapper( IQueryable query )
			{
				this.query = query;
			}

			public ObjectContext ObjectContext
			{
				get { return objectContext ?? ( objectContext = DetermineContext() ); }
			}	ObjectContext objectContext;

			ObjectContext DetermineContext()
			{
				var internalQuery = query.GetType()
				                         .GetFields( BindingFlags.NonPublic | BindingFlags.Instance )
				                         .Where( field => field.Name == "_internalQuery" )
				                         .Select( field => field.GetValue( query ) )
				                         .First();
				var objectQuery = internalQuery.GetType()
				                               .GetFields( BindingFlags.NonPublic | BindingFlags.Instance )
				                               .Where( field => field.Name == "_objectQuery" )
				                               .Select( field => field.GetValue( internalQuery ) )
				                               .Cast<ObjectQuery>()
				                               .First();
				return objectQuery.Context;
			}
		}

		static IEnumerable<string> DetermineDefaultAssociationProperties( IObjectContextAdapter target, Type type )
		{
			var names = GetAssociationPropertyNames( target, type );
			var decorated = type.GetProperties().Where( x => x.IsDecoratedWith<DefaultIncludeAttribute>() ).Select( x => x.Name );
			var result = decorated.Union( names ).ToArray();
			return result;
		}

		static IEnumerable<string> DetermineDefaultAssociationPaths( IObjectContextAdapter target, Type type, bool includeOtherPath = true )
		{
			var names = GetAssociationPropertyNames( target, type );
			var decorated = type.GetProperties().Where( x => x.IsDecoratedWith<DefaultIncludeAttribute>() ).SelectMany( x => includeOtherPath ? x.FromMetadata<DefaultIncludeAttribute, IEnumerable<string>>( y => y.AlsoInclude == "*" ? DetermineDefaultAssociationPaths( target, x.PropertyType, false ) : y.AlsoInclude.ToStringArray() ).Select( z => string.Concat( x.Name, ".", z ) ).Transform( a => a.Any() ? a : x.Name.AsItem() ) : x.Name.AsItem() ).ToArray();
			var result = decorated.Union( names.Where( x => !decorated.Any( y => y.StartsWith( string.Concat( x, "." ) ) ) ) ).ToArray();
			return result;
		}

		static IEnumerable<string> GetAssociationPropertyNames( IObjectContextAdapter target, Type type )
		{
			var propertyInfos = target.GetEntityProperties( type ).Select( x => type.GetProperty( x.Name ) );
			var names = propertyInfos.Where( x => x.GetCollectionType() == null ).Select( x => x.Name );
			return names;
		}

		public static TItem Entity<TItem>( this DbContext target, TItem item ) where TItem : class
		{
			switch ( target.Entry( item ).State )
			{
				case EntityState.Detached:
					using ( target.Configured( x => x.AutoDetectChangesEnabled = false ) )
					{
						var get = target.Get<TItem>( item );
						var result = get ?? Add( target, item );
						return result;
					}
			}
			return item;
		}

		static TItem Add<TItem>( DbContext target, TItem item ) where TItem : class
		{
			var properties = GetAssociationPropertyNames( target, typeof(TItem) );
			properties.Apply( x =>
			{
				var property = typeof(TItem).GetProperty( x );
				var current = property.GetValue( item );
				current.NotNull( y =>
				{
					switch ( target.Entry(y).State )
					{
						case EntityState.Detached:
							var entity = Get( target, y, property.PropertyType );
							property.SetValue( item, entity );
							break;
					}
				} );
			} );

			var result = target.Set<TItem>().Add( item );
			return result;
		}

		/*public static TItem Find<TItem>( this DbContext target, object container ) where TItem : class
		{
			var key = target.DetermineKey<TItem>( container ).Select( x => x.Value ).ToArray();
			var result = target.Set<TItem>().Find( key ).Transform( x => target.Include( x, DetermineDefaultNavigationProperties( target, typeof(TItem) ).ToArray() ) );
			return result;
		}*/

		public static TItem Create<TItem>( this DbContext target, Action<TItem> with = null ) where TItem : class, new()
		{
			var item = new TItem().WithDefaults().With( with );
			var result = target.Set<TItem>().Add( item );
			return result;
		}

		

		/*public static T Add<T>( this DbContext context, T entity ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( entity != null );
			var dbEntityEntry = context.Entry( entity );
			dbEntityEntry.State = EntityState.Added;
			return entity;
		}

		public static T Update<T>( this DbContext context, T entity ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( entity != null );
			var entry = context.Entry( entity );
			EnsureAttached( context, entity, entry );
			entry.State = EntityState.Modified;
			return entity;
		}

		static void EnsureAttached<T>( DbContext context, T entity, DbEntityEntry<T> entry ) where T : class
		{
			switch ( entry.State )
			{
				case EntityState.Detached:
					context.Set<T>().Attach( entity );
					break;
			}
		}

		public static T Update<T>( this DbContext context, T current, T original ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( current != null );
			Contract.Requires( original != null );

			var entry = context.Entry( current );
			EnsureAttached( context, current, entry );
			entry.State = EntityState.Unchanged;
			entry.OriginalValues.SetValues( original );
			var properties = TypeDescriptor.GetProperties( typeof(T) );
			// var attributes = TypeDescriptor.GetAttributes( typeof(T) );

			foreach ( var propertyName in from propertyName in entry.CurrentValues.PropertyNames
			                              let descriptor = properties[ propertyName ]
			                              /*where
											descriptor != null && descriptor.Attributes[ typeof(RoundtripOriginalAttribute) ] == null && 
											attributes[ typeof(RoundtripOriginalAttribute) ] == null && 
											descriptor.Attributes[ typeof(ExcludeAttribute) ] == null#1#
			                              select propertyName )
			{
				entry.Property( propertyName ).IsModified = true;
			}

			if ( entry.State != EntityState.Modified )
			{
				entry.State = EntityState.Modified;
			}
			return current;
		}

		public static object ApplyChanges( this DbContext target, object entity )
		{
			var result = ApplyChangesMethod.MakeGenericMethod( entity.GetType() ).Invoke( null, new[] { target, entity } );
			return result;
		}

		public static TEntity ApplyChanges<TEntity>( this DbContext target, TEntity entity ) where TEntity : class
		{
			return ApplyChangesTo( target, entity );
		}

		static TEntity ApplyChangesTo<TEntity>( DbContext target, TEntity entity ) where TEntity : class
		{
			var values = target.DetermineKey( entity );
			var set = target.Set<TEntity>();
			var current = set.Find( values );
			if ( current != null )
			{
				// var existing = target.Entry( entity );

				var entry = target.Entry( current );
				entry.CurrentValues.SetValues( entity );
				switch ( entry.State )
				{
					case EntityState.Unchanged:
					case EntityState.Detached:
						target.Set<TEntity>().Attach( entity );
						// Update( target, current );
						break;
				}
				return current;
			}
			
			target.Add( entity );
			return entity;
		}*/

		public static void Remove<T>( this DbContext context, T entity, bool clearProperties = true ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( entity != null );

			if ( clearProperties )
			{
				var type = entity.GetType();
				var properties = context.GetEntityProperties( type ).Where( x => x.FromEndMember.DeleteBehavior == OperationAction.Cascade ).Select( x => x.Name ).ToArray();
				Load( context, entity, properties );

				properties.Apply( x =>
				{
					var property = type.GetProperty( x );
					var raw = property.GetValue( entity );
					var items = property.GetCollectionType() != null ? raw.To<IEnumerable>().Cast<object>().ToArray() : new[] { raw };
					items.Apply( y => context.Set( y.GetType() ).Remove( y ) );
					context.SaveChanges();
				} );
			}

			context.Set<T>().Remove( entity );
		}

		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "target", Justification = "Used as a convenience to keep from adding another extension method to the object class." )]
        public static object[] ResolveKeyValues( this DbContext target, object entity )
		{
			var type = entity.GetType();
			var propertyInfos = type.GetProperties().Where( x => x.IsDecoratedWith<KeyAttribute>() ).OrderBy( x => x.FromMetadata<DisplayAttribute, int>( y => y.GetOrder().GetValueOrDefault( 0 ), () => 0 ) );
			var result = propertyInfos.Select( x => type.GetProperty( x.Name ).GetValue( entity, null ) ).ToArray();
			return result;
		}


		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "target", Justification = "Used as a convenience to keep from adding another extension method to the object class." )]
		public static IDictionary<string, object> DetermineKey<TEntity>( this DbContext target, object container )
		{
			var names = target.DetermineKeyNames<TEntity>();
			var result = names.Select( name =>
			{
				var info = container.GetType().GetProperty( name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
				var value = typeof(TEntity).GetProperty( name ).FromMetadata<ForeignKeyAttribute, object>( y =>
				{
					var propertyInfo = container.GetType().GetProperty( y.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
					var o = propertyInfo.GetValue( container );
					return o.Transform( z => {
						var objectStateEntry = target.AsTo<IObjectContextAdapter, ObjectContext>( x => x.ObjectContext ).ObjectStateManager.GetObjectStateEntry( z );
						return objectStateEntry.EntityKey.EntityKeyValues.First().Value;
					} );
				} ) ?? info.GetValue( container );
				return new { name, value };
			} ).ToDictionary( x => x.name, x => x.value );
			return result;
		}

		public static string[] DetermineKeyNames<TEntity>( this IObjectContextAdapter target )
		{
			var result = target.DetermineKeyNames( typeof(TEntity) );
			return result;
		}

		public static string[] DetermineKeyNames( this IObjectContextAdapter target, Type type )
		{
			var entitySet = target.ObjectContext.DetermineEntitySet( type );
			var result = entitySet.Transform( x => x.ElementType.KeyMembers.Select( y => y.Name ).ToArray() );
			return result;
		}

		public static IEnumerable<NavigationProperty> GetEntityProperties( this IObjectContextAdapter target, Type type )
		{
			var entityType = target.ObjectContext.MetadataWorkspace.GetEntityMetaData( type );
			var result = entityType.NavigationProperties;
			return result;
		}

		public static Type[] GetDeclaredEntityTypes( this DbContext context )
		{
			var result = context.GetType().GetProperties().Where( x => x.PropertyType.IsGenericType && typeof(DbSet<>).IsAssignableFrom( x.PropertyType.GetGenericTypeDefinition() ) ).Select( x => x.PropertyType.GetGenericArguments().FirstOrDefault() ).NotNull().ToArray();
			return result;
		}

		public static IDisposable Configured<TContext>( this TContext target, Action<DbContextConfiguration> configure ) where TContext : DbContext
		{
			var result = new ConfigurationContext( target, configure );
			return result;
		}

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, params Expression<Func<TEntity, object>>[] expressions ) where TEntity : class
		{
			var result = target.Include( entity, 1, expressions );
			return result;
		}

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, int levels, params Expression<Func<TEntity, object>>[] expressions ) where TEntity : class
		{
			var result = target.Include( entity, expressions.Select( x => x.GetMemberInfo().Name ).ToArray(), levels );
			return result;
		}

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, string[] associationNames, int levels = 1 ) where TEntity : class
		{
			var associations = associationNames ?? Enumerable.Empty<string>();
			var names = associations.Union( DetermineDefaultAssociationProperties( target, typeof(TEntity) ) ).ToArray();
			var result = Load( target, entity, names, levels );
			return result;
		}

		public static TItem Load<TItem>( this DbContext target, TItem entity, string[] properties = null, int? levels = 1, bool? loadAllProperties = null )
		{
			using ( target.Configured( x => x.AutoDetectChangesEnabled = false ) )
			{
				LoadAll( target, entity, new ArrayList(), null, loadAllProperties.GetValueOrDefault( levels == 1 ), levels, 0 );
				return entity;
			}
		}

		static void LoadAll( DbContext target, object entity, IList list, IEnumerable<string> properties, bool loadAllProperties, int? levels, int count )
		{
			if ( !list.Contains( entity ) )
			{
				list.Add( entity );
				var type = entity.GetType();
				var names = properties ?? ( loadAllProperties ? target.GetEntityProperties( type ).Select( x => x.Name ) : DetermineDefaultAssociationProperties( target, type ) );
				var associationNames = names.ToArray();
				LoadEntity( target, entity, associationNames );

				if ( !levels.HasValue || ++count < levels.Value )
				{
					associationNames.Select( y => type.GetProperty( y ).GetValue( entity ) ).NotNull().Apply( z =>
					{
						var items = z.GetType().GetCollectionElementType() != null ? z.AsTo<IEnumerable, object[]>( a => a.Cast<object>().ToArray() ) : z.AsItem();
						items.Apply( a => LoadAll( target, a, list, null, loadAllProperties, levels, count ) );
					} );
					count--;
				}
			}
		}

		static void LoadEntity( DbContext target, object entity, IEnumerable<string> associationNames )
		{
			var entry = target.Entry( entity );
			if ( entry.State != EntityState.Added )
			{
				foreach ( var name in associationNames )
				{
					if ( entity.GetType().GetProperty( name ).GetCollectionType() != null )
					{
						var collection = entry.Collection( name );
						var current = collection.CurrentValue.To<IEnumerable>().Cast<object>();
						var canLoad = !collection.IsLoaded && current.All( x => target.Entry( x ).State != EntityState.Added );
						try
						{
							canLoad.IsTrue( collection.Load );
						}
						catch ( InvalidOperationException )
						{}
					}
					else
					{
						var reference = entry.Reference( name );
						reference.IsLoaded.IsFalse( reference.Load );
					}
				}
			}
		}

		/*public static IQueryable<TUser> GetAdministrators<TUser>( this EntityStorage<TUser> target ) where TUser : User
		{
			var result = target.Users.Where( x => x.RolesSource.Contains( "Administrator" ) );
			return result;
		}*/

		[SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract the expression." )]
		public static IEnumerable<TEntity> Total<TEntity>( this DbContext context, Expression<Func<TEntity, bool>> predicate ) where TEntity : class
		{
			var dbSet = context.Set<TEntity>();
			var result = dbSet.Where( predicate ).ToArray().Union( dbSet.Local ).Where( predicate.Compile() ).ToArray();
			return result;
		}

		class ConfigurationContext : IDisposable
		{
			readonly List<Tuple<bool, Action<bool>>> saved;

			public ConfigurationContext( DbContext context, Action<DbContextConfiguration> configure )
			{
				saved = new List<Tuple<bool, Action<bool>>>
					{
						new Tuple<bool, Action<bool>>( context.Configuration.AutoDetectChangesEnabled, x => context.Configuration.AutoDetectChangesEnabled = x ),
						new Tuple<bool, Action<bool>>( context.Configuration.LazyLoadingEnabled, x => context.Configuration.LazyLoadingEnabled = x ),
						new Tuple<bool, Action<bool>>( context.Configuration.ProxyCreationEnabled, x => context.Configuration.ProxyCreationEnabled = x ),
						new Tuple<bool, Action<bool>>( context.Configuration.ValidateOnSaveEnabled, x => context.Configuration.ValidateOnSaveEnabled = x )
					};
				configure( context.Configuration );
			}

			public void Dispose()
			{
				saved.Apply( x => x.Item2( x.Item1 ) );
			}
		}
	}

	/*public static class Database
	{
		public static void Initialize<TContext>( IDatabaseInitializer<TContext> initializer ) where TContext : System.Data.Entity.DbContext, new()
		{
			System.Data.Entity.Database.SetInitializer( initializer );
			using ( var context = new TContext() )
			{
				context.Database.Initialize(true);
			}
		}
	}*/

	[AttributeUsage( AttributeTargets.Property )]
	public class DefaultIncludeAttribute : Attribute
	{
		public string AlsoInclude { get; set; }
	}

	public class EntityKeyExpressionBuilder<TItem> : Factory<Expression<Func<TItem, bool>>>
	{
		readonly IDictionary<string, object> key;

		public EntityKeyExpressionBuilder( IDictionary<string, object> key )
		{
			this.key = key;
		}

		protected override Expression<Func<TItem, bool>> CreateItem( object source )
		{
			var parameter = Expression.Parameter( typeof(TItem), "x" );
			var first = key.FirstOrDefault().Transform( x => CreateMethod( parameter, x.Key, x.Value ) );
			var expression = key.Skip( 1 ).Aggregate( first, ( current, item ) => Expression.And( current, CreateMethod( parameter, item.Key, item.Value ) ) );
			var result = Expression.Lambda<Func<TItem, bool>>( expression, parameter );
			return result;
		}

		static Expression CreateMethod( Expression parameter, string propertyName, object value )
		{
			var property = Expression.Property( parameter, propertyName );
			var propertyType = typeof(TItem).GetProperty( propertyName ).PropertyType;
			var constantExpression = value.Transform( x => Expression.Constant( x, propertyType ) ) ?? (Expression)Expression.Default( propertyType );
			var result = Expression.Equal( property, constantExpression );
			return result;
		}
	}

	public static class ObjectContextExtensions
	{
		readonly static List<ObjectContext> InitializedContexts = new List<ObjectContext>();

		public static TContext Initialized<TContext>( this TContext target ) where TContext : ObjectContext
		{
			if ( !InitializedContexts.Contains( target ) )
			{
				target.MetadataWorkspace.LoadFromAssembly( target.GetType().Assembly );
				InitializedContexts.Add( target );
			}
			return target;
		}

		public static string CreateQueryText<TEntity>( this ObjectContext target, string where ) where TEntity : EntityObject
		{
			var result = target.CreateQueryText( typeof(TEntity), where );
			return result;
		}
	
		public static string CreateQueryText( this ObjectContext target, Type entityType, string where )
		{
			var result = string.Format( "SELECT VALUE entity FROM {0}.{1} AS entity WHERE {2}", target.DefaultContainerName,
			                            target.DetermineEntitySet( entityType ).Name, where );
			return result;
		}


		/*public static IEnumerable<EntitySet> ResolveEntitySets<TEntity>( this ObjectContext context )
		{
			var result = context.ResolveEntitySets( typeof(TEntity) );
			return result;
		}

		public static IEnumerable<EntitySet> ResolveEntitySets( this ObjectContext context, Type entityType )
		{
			var collection = context.MetadataWorkspace.GetItemCollection( DataSpace.OSpace );
			var items = collection.To<ObjectItemCollection>();

			var types = items.GetItems<TEntity>();
			var result = from item in types
                         where items.GetClrType( item ) == entityType
						 let type = context.MetadataWorkspace.GetEdmSpaceType( item )
                         from container in context.MetadataWorkspace.GetItems<EntityContainer>( DataSpace.CSpace )
                         from set in container.BaseEntitySets.OfType<EntitySet>()
                         where set.ElementType.Equals( type )
                         select set;
			return result;
		}
		 internal static EntitySetBase GetEntitySet<TEntity>(this ObjectContext context)
        {
            EntityContainer container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);

            EntitySetBase entitySet = container.BaseEntitySets.Where(item => item.ElementType.Name.Equals(typeof(TEntity).Name))
                                                              .FirstOrDefault();

            return entitySet;
        }
*/
		public static TItem Find<TItem>( this ObjectContext target, object[] key ) where TItem : class
		{
			var entityKey = MetadataHelper.CreateKey<TItem>( target, key );
			object entity;
			var result = target.TryGetObjectByKey( entityKey, out entity ) ? (TItem)entity : null;
			return result;
		}

		public static EntitySet DetermineEntitySet<TEntity>( this ObjectContext context )
		{
			var result = context.DetermineEntitySet( typeof(TEntity) );
			return result;
		}

		public static EntitySet DetermineEntitySet( this ObjectContext context, Type entityType )
		{
			/*var propertyInfos = context.GetType().GetProperties();
			var query = from property in propertyInfos
			            where property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObjectSet<>)
			            where property.PropertyType.GetGenericArguments().First().IsAssignableFrom( entityType )
			            let container = context.MetadataWorkspace.GetEntityContainer( context.DefaultContainerName, DataSpace.CSpace )
			            select container.GetEntitySetByName( property.Name, true );
			var result = query.Single();*/
			var objects = (ObjectItemCollection)context.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);
			var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
			var result = container.BaseEntitySets.OfType<EntitySet>().FirstOrDefault(x => !x.Name.Contains( "_" ) && objects.GetClrType( context.MetadataWorkspace.GetObjectSpaceType( x.ElementType ) ).IsAssignableFrom( entityType ));
			return result;
		}

		public static void AddOrAttach( this ObjectContext target, object entity )
		{
			object existing;
			var key = MetadataHelper.ExtractKey( target, entity );
			if ( target.TryGetObjectByKey( key, out existing ) )
			{
				var entityWithKey = entity.To<IEntityWithKey>();
				entityWithKey.EntityKey = key;
				target.Detach( existing );
				target.Attach( entityWithKey );
			}
			else
			{
				target.Add( entity );
			}
		}

		public static void Add( this ObjectContext target, object entity )
		{
			var entitySet = target.DetermineEntitySet( entity.GetType() );
			target.AddObject( entitySet.Name, entity );
		}

		public static IQueryable GetQuery( this ObjectContext target, Type entityType )
		{
			var entitySet = target.DetermineEntitySet( entityType );
			var query = target.GetType().GetProperty( entitySet.Name ).GetValue( target, null );
			var result = query.GetType().GetMethod( "OfType" ).MakeGenericMethod( entityType ).Invoke( query, null ).As<IQueryable>();
			return result;
		}

		public static ObjectQuery<TEntity> GetQuery<TEntity>( this ObjectContext target )
		{
			var entitySet = target.DetermineEntitySet<TEntity>();
			var query = target.GetType().GetProperty( entitySet.Name ).GetValue( target, null );
			var result = query.GetType().GetMethod( "OfType" ).MakeGenericMethod( typeof(TEntity) ).Invoke( query, null ).As<ObjectQuery<TEntity>>();
			return result;
		}

		public static void DeleteObjectEnsured( this ObjectContext target, object entity )
		{
			// HACK: Ensure all ends are loaded:
			var query = from property in entity.GetType().GetProperties( DragonSparkBindingOptions.AllProperties )
			            where typeof(IRelatedEnd).IsAssignableFrom( property.PropertyType )
			            let end = property.GetValue( entity, null ) as IRelatedEnd
			            where end != null && !end.IsLoaded
			            select end;
			foreach ( var end in query )
			{
				end.Load();
			}

			target.DeleteObject( entity );
		}
	}

	public static class MetadataHelper
	{
		static readonly Dictionary<Type, EntityType> EntityTypes = new Dictionary<Type, EntityType>();

		public static EntityType GetEntityMetaData<T>( this ObjectContext context )
		{
			return GetEntityMetaData( context.MetadataWorkspace, typeof(T) );
		}

		public static EntityType GetEntityMetaData( this MetadataWorkspace workspace, Type entityType )
		{
			lock ( EntityTypes )
			{
				if ( !EntityTypes.ContainsKey( entityType ) )
				{
					var type = ResolveType( workspace, entityType );
					EntityTypes.Add( entityType, type );
				}
				return EntityTypes[ entityType ];
			}
		}

		static EntityType ResolveType( MetadataWorkspace workspace, Type type )
		{
			try
			{
				var result = (EntityType)workspace.GetType( type.Name, type.Namespace, true, DataSpace.CSpace );
				return result;
			}
			catch
			{
				var namespaceName =
					workspace.GetItems( DataSpace.CSpace ).OfType<EntityContainer>().First().BaseEntitySets.First().ElementType.
						NamespaceName;
				var name = type.AssemblyQualifiedName.Contains( "EntityFrameworkDynamicProxies" ) ? type.BaseType.Name : type.Name;
				var item = (EntityType)workspace.GetType( name, namespaceName, true, DataSpace.CSpace );
				return item;
			}
		}

		public static IEnumerable<EntityType> GetHierarchy( this EntityType target )
		{
			var result = new List<EntityType> { target };
			var current = target.BaseType as EntityType;
			while ( current != null )
			{
				result.Add( current );
				current = current.BaseType as EntityType;
			}
			return result;
		}

		public static EntityKey CreateKey<T, U>( ObjectContext context, U id ) where T : EntityObject where U : struct
		{
			var type = GetEntityMetaData<T>( context );
			var list = new List<EntityKeyMember> { new EntityKeyMember( new List<EdmMember>( type.KeyMembers )[ 0 ].Name, id ) };
			return CreateKey( context, typeof(T), list );
		}

		public static EntityKey CreateKey<T>( ObjectContext context, params object[] values )
		{
			var type = GetEntityMetaData( context.MetadataWorkspace, typeof(T) );
			var list = new List<EntityKeyMember>( type.KeyMembers.Select( x => new EntityKeyMember( x.Name, values[ type.KeyMembers.IndexOf( x ) ] ) ) );
			return CreateKey( context, typeof(T), list );
		}

		public static EntityKey CreateKey( ObjectContext context, Type entityType,
		                                   IEnumerable<EntityKeyMember> pairs )
		{
			var name = string.Concat( context.DefaultContainerName, ".", context.DetermineEntitySet( entityType ) );
			var result = new EntityKey( name, pairs );
			return result;
		}

		public static EntityKey CreateKey<T>( ObjectContext context, IOrderedDictionary values ) where T : EntityObject
		{
			var items = new List<EntityKeyMember>();
			var metadata = GetEntityMetaData<T>( context );
			foreach ( var property in ConvertProperties<EdmProperty, EdmMember>( metadata.KeyMembers ) )
			{
				if ( values.Contains( property.Name ) )
				{
					items.Add( new EntityKeyMember( property.Name, values[ property.Name ] ) );
				}
				else
				{
					throw new ArgumentNullException( "key", "A property that's part of a key was not found." );
				}
			}
			return CreateKey( context, typeof(T), items );
		}

		public static EntityKey ExtractKey( ObjectContext context, object target )
		{
			return ExtractKey( context, target.GetType(), target );
		}

		public static EntityKey ExtractKey( ObjectContext context, Type type, object target )
		{
			var metadata = GetEntityMetaData( context.Initialized().MetadataWorkspace, type );
			try
			{
				var pairs = ExtractKeyValues( metadata, target );
				var result = CreateKey( context, type, pairs );
				return result;
			}
			catch
			{
				return null;
			}
		}

		public static List<EntityKeyMember> ExtractKeyValues( EntityType type, object target )
		{
			var dictionary = target as IDictionary;
			var targetType = target.GetType();
			var result = new List<EntityKeyMember>();
			foreach ( var property in ConvertProperties<EdmProperty, EdmMember>( type.KeyMembers ) )
			{
				var value = dictionary != null && dictionary.Contains( property.Name )
					            ? dictionary[ property.Name ]
					            : targetType.GetProperty( property.Name ).GetValue( target, null );
				result.Add( new EntityKeyMember( property.Name, value ) );
			}
			return result;
		}

		internal static IEnumerable<T> ConvertProperties<T, U>( IEnumerable<U> members ) where T : MetadataItem
			where U : MetadataItem
		{
			var result = from member in members.OfType<T>()
			             select member;
			return result;
		}

		internal static IEnumerable<T> ConvertProperties<T>( IEnumerable<MetadataItem> members ) where T : MetadataItem
		{
			return ConvertProperties<T, MetadataItem>( members );
		}
	}
	
}