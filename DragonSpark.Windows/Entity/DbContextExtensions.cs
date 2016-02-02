using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using Type = System.Type;

namespace DragonSpark.Windows.Entity
{
	public static class DbContextExtensions
	{
		readonly static MethodInfo
			GetMethod = typeof(DbContextExtensions).GetMethod( nameof(Dynamitey.DynamicObjects.Get), new[] { typeof(DbContext), typeof(object), typeof(int) } ),
			ApplyChangesMethod = typeof(DbContextExtensions).GetMethods().FirstOrDefault( x => x.IsGenericMethod && x.Name == nameof(ApplyChanges) );

		/*public static TEntity Find<TEntity>( this DbContext @this, Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> with = null ) where TEntity : class
		{
			var entity = DbSetExtensions.Find( @this.Set<TEntity>(), @where, with );
			var result = entity.With( x => @this.Load( x, loadAllProperties: false ) );
			return result;
		}*/

		public static int Save( this DbContext target, bool? validate = null, bool? autoDetectChanges = null )
		{
			try
			{
				/*if ( validate.GetValueOrDefault( target.Configuration.ValidateOnSaveEnabled ) )
				{
					target.ChangeTracker.DetectChanges();
					// target.ApplyAllValues();
				}*/

				return target.SaveChanges();
			}
			catch ( DbEntityValidationException error )
			{
				throw new EntityValidationException( target, error );
			}
		}

		/*public static void ApplyAllValues( this DbContext @this )
		{
			using ( @this.Configured( x =>
			{
				x.AutoDetectChangesEnabled = false;
				x.ValidateOnSaveEnabled = false;
			} ) )
			{
				var entries = @this.ChangeTracker.Entries().Where( CanApply ).ToArray();
				// entries.Each( y => y.Entity.ApplyValues() );
			}
		}

		static bool CanApply( DbEntityEntry entityEntry )
		{
			switch ( entityEntry.State )
			{
				case EntityState.Added:
				case EntityState.Modified:
				case EntityState.Unchanged:
					return Services.IsAvailable() && entityEntry.Entity.GetType().GetProperties().Any( x => x.GetAttributes<ApplyValueAttribute>().Any() );
			}
			return false;
		}*/

		public static object ApplyChanges( this DbContext target, object entity )
		{
			var type = entity.GetType();
			var items = target.GetEntityProperties( type ).SelectMany( x => type.GetProperty( x.Name ).GetValue( entity ).With( o => o.AsTo<IEnumerable, IEnumerable<object>>( enumerable => enumerable.Cast<object>().Select( target.ApplyChanges ), o.ToItem ) ) );
			var all = entity.Prepend( items ).Distinct();
			all.Each( o =>
			{
				ApplyChangesMethod.MakeGenericMethod( o.GetType() ).Invoke( null, new[] { target, o } );
			} );
			return entity;
		}

		public static TEntity ApplyChanges<TEntity>( this DbContext target, TEntity entity ) where TEntity : class
		{
			var entityState = target.Entry( entity ).State;
			switch ( entityState )
			{
				case EntityState.Detached:
					var dbSet = target.Set<TEntity>();
					dbSet.AddOrUpdate( dbSet.Attach( entity ) );
					break;
			}
			return entity;
		}

		public static object Get( this DbContext target, object entity, Type entityType = null ) => GetMethod.MakeGenericMethod( entityType ?? entity.GetType() ).Invoke( null, new[] { target, entity, 1 } );

		public static TItem Get<TItem>( this DbContext target, object container, int levels = 1 ) where TItem : class
		{
			var key = new KeyFactory<TItem>( target ).Create( container );

			var current = target.Set<TItem>().Find( key.Values.ToArray() );

			var result = current.With( x => target.Include( x, levels ) );

			return result;

			/*using ( target.Configured( x => x.AutoDetectChangesEnabled = false ) )
			{}*/
		}

		/*public static IQueryable<TItem> WithIncludes<TItem>( this DbContext target ) where TItem : class => WithIncludes( target.Set<TItem>(), target );

		public static IQueryable<TItem> WithIncludes<TItem>( this IQueryable<TItem> target ) where TItem : class => WithIncludes( target, new QueryWrapper( target ) );

		static IQueryable<TItem> WithIncludes<TItem>( IQueryable<TItem> target, IObjectContextAdapter adapter ) where TItem : class
		{
			var names = new DefaultAssociationPathsFactory( AttributeProvider.Instance, adapter ).Create( typeof(TItem) );
			var result = names.Aggregate( target, ( current, item ) => current.Include( item ) );
			return result;
		}*/

		/*class QueryWrapper : IObjectContextAdapter
		{
			readonly IQueryable query;

			public QueryWrapper( IQueryable query )
			{
				this.query = query;
			}

			public ObjectContext ObjectContext => objectContext ?? ( objectContext = DetermineContext() );
			ObjectContext objectContext;

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
		}*/

		public class DefaultAssociationPropertyFactory : FactoryBase<Type, string[]>
		{
			readonly IObjectContextAdapter adapter;

			public DefaultAssociationPropertyFactory( IObjectContextAdapter adapter )
			{
				this.adapter = adapter;
			}

			protected override string[] CreateItem( Type parameter )
			{
				var names = GetAssociationPropertyNames( adapter, parameter );
				var decorated = parameter.GetProperties().Where( x => x.Has<DefaultIncludeAttribute>() ).Select( x => x.Name );
				var result = decorated.Union( names ).ToArray();
				return result;
			}

			static IEnumerable<string> GetAssociationPropertyNames( IObjectContextAdapter target, Type type ) => target.GetEntityProperties( type ).Select( x => type.GetProperty( x.Name ) ).Where( x => x.PropertyType.Adapt().GetInnerType() == null ).Select( x => x.Name );
		}

		/*public class DefaultAssociationPathsFactory : FactoryBase<Type, string[]>
		{
			readonly IAttributeProvider provider;
			readonly IObjectContextAdapter adapter;
			readonly bool includeOtherPaths;

			public DefaultAssociationPathsFactory( IAttributeProvider provider, IObjectContextAdapter adapter, bool includeOtherPaths = true )
			{
				this.provider = provider;
				this.adapter = adapter;
				this.includeOtherPaths = includeOtherPaths;
			}

			protected override string[] CreateItem( Type parameter )
			{
				var names = GetAssociationPropertyNames( adapter, parameter );
				var decorated = parameter.GetProperties()
								.Where( provider.IsDecoratedWith<DefaultIncludeAttribute> )
								.SelectMany( x => includeOtherPaths ? provider.FromMetadata<DefaultIncludeAttribute, IEnumerable<string>>( x, y => y.AlsoInclude == "*" ? new DefaultAssociationPathsFactory( provider, adapter, false ).CreateItem( x.PropertyType ) : y.AlsoInclude.ToStringArray() ).Select( z => string.Concat( x.Name, ".", z ) ).With( a => a.Any() ? a : x.Name.ToItem() ) : x.Name.ToItem() ).ToArray();
				var result = decorated.Union( names.Where( x => !decorated.Any( y => y.StartsWith( string.Concat( x, "." ) ) ) ) ).ToArray();
				return result;
			}
		}*/

		/*public static TItem Entity<TItem>( this DbContext target, TItem item ) where TItem : class
		{
			var state = target.Entry( item ).State;
			switch ( state )
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
		}*/

		/*static TItem Add<TItem>( DbContext target, TItem item ) where TItem : class
		{
			var properties = GetAssociationPropertyNames( target, typeof(TItem) );
			properties.Each( x =>
			{
				var property = typeof(TItem).GetProperty( x );
				var current = property.GetValue( item );
				current.With( y =>
				{
					switch ( target.Entry( y ).State )
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
		}*/

		public static TItem Create<TItem>( this DbContext target, Action<TItem> with = null ) where TItem : class, new()
		{
			var item = new TItem().With( with );
			var result = target.Set<TItem>().Add( item );
			return result;
		}

		public static void Remove<T>( this DbContext context, T entity, bool clearProperties = true ) where T : class
		{
			if ( clearProperties )
			{
				var type = entity.GetType();
				var properties = context.GetEntityProperties( type ).Where( x => x.FromEndMember.DeleteBehavior == OperationAction.Cascade ).Select( x => x.Name ).ToArray();
				Load( context, entity, properties );

				properties.Each( x =>
				{
					var property = type.GetProperty( x );
					var raw = property.GetValue( entity );
					var items = property.PropertyType.Adapt().GetInnerType() != null ? raw.To<IEnumerable>().Cast<object>().ToArray() : new[] { raw };
					items.Each( y => context.Set( y.GetType() ).Remove( y ) );
					context.Save();
				} );
			}

			context.Set<T>().Remove( entity );
		}

		/*[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "target", Justification = "Used as a convenience to keep from adding another extension method to the object class." )]
		public static object[] ResolveKeyValues( this DbContext target, object entity )
		{
			var type = entity.GetType();
			var propertyInfos = type.GetProperties().Where( x => x.IsDecoratedWith<KeyAttribute>() ).OrderBy( x => x.FromMetadata<DisplayAttribute, int>( y => y.GetOrder().GetValueOrDefault( 0 ), () => 0 ) );
			var result = propertyInfos.Select( x => type.GetProperty( x.Name ).GetValue( entity, null ) ).ToArray();
			return result;
		}*/

		class KeyFactory<TEntity> : FactoryBase<object, IDictionary<string, object>>
		{
			readonly IObjectContextAdapter context;

			public KeyFactory( [Required] IObjectContextAdapter context )
			{
				this.context = context;
			}

			protected override IDictionary<string, object> CreateItem( object parameter )
			{
				var names = context.ObjectContext.DetermineEntitySet( typeof(TEntity) ).With( x => x.ElementType.KeyMembers.Select( y => y.Name ).ToArray() );
				return parameter.GetType().IsPrimitive ? new Dictionary<string, object> { { names.First(), parameter } } : DetermineKeyComplex( context, parameter, names );
			}

			IDictionary<string, object> DetermineKeyComplex( IObjectContextAdapter target, object container, string[] names )
			{
				var result = names.Select( name =>
				{
					var info = container.GetType().GetProperty( name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
					var value = typeof(TEntity).GetProperty( name ).From<ForeignKeyAttribute, object>( y =>
					{
						var propertyInfo = container.GetType().GetProperty( y.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
						var o = propertyInfo.GetValue( container );
						return o.With( z =>
						{
							var objectStateEntry = target.AsTo<IObjectContextAdapter, ObjectContext>( x => x.ObjectContext ).ObjectStateManager.GetObjectStateEntry( z );
							return objectStateEntry.EntityKey.EntityKeyValues.First().Value;
						} );
					} ) ?? info.GetValue( container );
					return new { name, value };
				} ).ToDictionary( x => x.name, x => x.value );
				return result;
			}
		}
		
		public static IEnumerable<NavigationProperty> GetEntityProperties( this IObjectContextAdapter target, Type type ) => target.ObjectContext.MetadataWorkspace.GetEntityMetaData( type ).NavigationProperties;

		public static Type[] GetDeclaredEntityTypes( this DbContext context ) => context.GetType().GetProperties().Where( x => x.PropertyType.IsGenericType && typeof( DbSet<> ).IsAssignableFrom( x.PropertyType.GetGenericTypeDefinition() ) ).Select( x => x.PropertyType.GetGenericArguments().FirstOrDefault() ).NotNull().ToArray();

		// public static IDisposable Configured<TContext>( this TContext target, Action<DbContextConfiguration> configure ) where TContext : DbContext => new ConfigurationContext( target, configure );

		/*public static TEntity Include<TEntity>( this DbContext target, TEntity entity, params Expression<Func<TEntity, object>>[] expressions ) where TEntity : class
		{
			var result = target.Include( entity, 1, expressions );
			return result;
		}*/

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, int levels, params Expression<Func<TEntity, object>>[] expressions ) where TEntity : class => target.Include( entity, expressions.Select( x => x.GetMemberInfo().Name ).ToArray(), levels );

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, string[] associationNames, int levels = 1 ) where TEntity : class
		{
			var associations = associationNames ?? Enumerable.Empty<string>();
			var names = associations.Union( new DefaultAssociationPropertyFactory( target ).Create( typeof(TEntity) ) ).ToArray();
			var result = Load( target, entity, names, levels );
			return result;
		}

		public static TItem Load<TItem>( this DbContext target, TItem entity, string[] properties = null, int? levels = 1, bool? loadAllProperties = null )
		{
			LoadAll( target, entity, new ArrayList(), properties, loadAllProperties.GetValueOrDefault( levels == 1 ), levels, 0 );
			return entity;
			/*using ( target.Configured( x => x.AutoDetectChangesEnabled = false ) )
			{
				
			}*/
		}

		static void LoadAll( DbContext target, object entity, IList list, IEnumerable<string> properties, bool loadAllProperties, int? levels, int count )
		{
			if ( !list.Contains( entity ) )
			{
				list.Add( entity );
				var type = entity.GetType();
				var names = properties ?? ( loadAllProperties ? target.GetEntityProperties( type ).Select( x => x.Name ) : new DefaultAssociationPropertyFactory( target ).Create( type ) );
				var associationNames = names.ToArray();
				LoadEntity( target, entity, associationNames );

				if ( !levels.HasValue || ++count < levels.Value )
				{
					associationNames.Select( y => type.GetProperty( y ).GetValue( entity ) ).NotNull().Each( z =>
					{
						var items = z.Adapt().GetInnerType() != null ? z.AsTo<IEnumerable, object[]>( a => a.Cast<object>().ToArray() ) : z.ToItem();
						items.Each( a => LoadAll( target, a, list, null, loadAllProperties, levels, count ) );
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
					if ( entity.GetType().GetProperty( name ).PropertyType.Adapt().GetInnerType() != null )
					{
						var collection = entry.Collection( name );
						var current = collection.CurrentValue.AsTo<IEnumerable, IEnumerable<object>>( x => x.Cast<object>() );
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

		/*[SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract the expression." )]
		public static int Total<TEntity>( this DbContext context, Expression<Func<TEntity, bool>> predicate, EntityState? state = EntityState.Added ) where TEntity : class
		{
			var dbSet = context.Set<TEntity>();
			var result = dbSet.Count( predicate ) + dbSet.Local.Where( x =>	 context.Entry( x ).State == state ).Count( predicate.Compile() );
			return result;
		}*/

		/*class ConfigurationContext : IDisposable
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
				saved.Each( x => x.Item2( x.Item1 ) );
			}
		}*/

		/*public static ObjectStateEntry GetEntry( this DbContext @this, object entity )
		{
			var objectStateManager = @this.AsTo<IObjectContextAdapter, ObjectStateManager>( x => x.ObjectContext.ObjectStateManager );
			var result = objectStateManager.GetObjectStateEntry( entity );
			return result;
		}*/

		/*public static string[] GetModifiedProperties( this DbContext @this, object entity )
		{
			var stateEntry = @this.GetEntry( entity );
			var currentValues = stateEntry.CurrentValues;
			var originalValues = stateEntry.OriginalValues;
			var result = stateEntry.GetModifiedProperties().Where( x => !originalValues.GetValue( originalValues.GetOrdinal( x ) ).Equals( currentValues.GetValue( currentValues.GetOrdinal( x ) ) ) ).ToArray();
			return result;
		}*/
	}
}