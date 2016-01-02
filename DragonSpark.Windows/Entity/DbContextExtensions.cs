using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace DragonSpark.Windows.Entity
{
	public static class DbContextExtensions
	{
		readonly static MethodInfo
			GetMethod = typeof(DbContextExtensions).GetMethod( "Get", new[] { typeof(DbContext), typeof(object), typeof(int) } ),
			ApplyChangesMethod = typeof(DbContextExtensions).GetMethods().FirstOrDefault( x => x.IsGenericMethod && x.Name == "ApplyChanges" );

		public static TEntity Find<TEntity>( this DbContext @this, Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> with = null ) where TEntity : class
		{
			var entity = DbSetExtensions.Find( @this.Set<TEntity>(), @where, with );
			var result = entity.With( x => @this.Load( x, loadAllProperties: false ) );
			return result;
		}

		readonly static IList<DbContext> Saving = new List<DbContext>();

		public static int EnsureSaved( this DbContext target )
		{
			lock ( target	 )
			{
				return !Saving.Contains( target ) ? target.Save() : 0;
			}
		}

		public static int Save( this DbContext target, bool? validate = null, bool? autoDetectChanges = null )
		{
			lock ( target )
			{
				var remove = !Saving.Contains( target );
				if ( remove )
				{
					Saving.Add( target );
				}
				try
				{
					if ( validate.GetValueOrDefault( target.Configuration.ValidateOnSaveEnabled ) )
					{
						target.ChangeTracker.DetectChanges();
						// target.ApplyAllValues();
					}

					using ( target.Configured( x =>
					{
						x.ValidateOnSaveEnabled = validate ?? x.ValidateOnSaveEnabled;
						x.AutoDetectChangesEnabled = autoDetectChanges ?? x.AutoDetectChangesEnabled;
					} ) )
					{
						return target.SaveChanges();
					}
				}
				catch ( DbEntityValidationException error )
				{
					throw new EntityValidationException( target, error );
				}
				finally
				{
					if ( remove )
					{
						Saving.Remove( target );
					}
				}
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

		public static object Get( this DbContext target, object entity, Type entityType = null )
		{
			var result = GetMethod.MakeGenericMethod( entityType ?? entity.GetType() ).Invoke( null, new[] { target, entity, 1 } );
			return result;
		}

		public static TItem Get<TItem>( this DbContext target, object container, int levels = 1 ) where TItem : class
		{
			using ( target.Configured( x => x.AutoDetectChangesEnabled = false ) )
			{
				var key = target.DetermineKey<TItem>( container );
					
				var current = target.Set<TItem>().Find( key.Values.ToArray() );
				
				var result = current.With( x => target.Include( x, levels ) );

				return result;
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
			var decorated = type.GetProperties().Where( x => x.IsDecoratedWith<DefaultIncludeAttribute>() ).SelectMany( x => includeOtherPath ? x.FromMetadata<DefaultIncludeAttribute, IEnumerable<string>>( y => y.AlsoInclude == "*" ? DetermineDefaultAssociationPaths( target, x.PropertyType, false ) : y.AlsoInclude.ToStringArray() ).Select( z => string.Concat( x.Name, ".", z ) ).With( a => a.Any() ? a : x.Name.ToItem() ) : x.Name.ToItem() ).ToArray();
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
		}

		static TItem Add<TItem>( DbContext target, TItem item ) where TItem : class
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
		}

		public static TItem Create<TItem>( this DbContext target, Action<TItem> with = null, Action<TItem> initialize = null ) where TItem : class, new()
		{
			var item = new TItem().With( initialize ).BuildUp().With( with );
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
					var items = property.GetCollectionType() != null ? raw.To<IEnumerable>().Cast<object>().ToArray() : new[] { raw };
					items.Each( y => context.Set( y.GetType() ).Remove( y ) );
					context.Save();
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
			return container.GetType().IsPrimitive ? DetermineKeySimple<TEntity>( target, container ) : DetermineKeyComplex<TEntity>( target, container );
		}

		static IDictionary<string, object> DetermineKeySimple<T>( DbContext target, object container )
		{
			var names = target.DetermineKeyNames<T>();
			var result = new Dictionary<string, object> { { names.First(), container } };
			return result;
		}

		static IDictionary<string, object> DetermineKeyComplex<TEntity>( DbContext target, object container )
		{
			var names = target.DetermineKeyNames<TEntity>();
			var result = names.Select( name =>
			{
				var info = container.GetType().GetProperty( name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
				var value = typeof(TEntity).GetProperty( name ).FromMetadata<ForeignKeyAttribute, object>( y =>
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

		public static string[] DetermineKeyNames<TEntity>( this IObjectContextAdapter target )
		{
			var result = target.DetermineKeyNames( typeof(TEntity) );
			return result;
		}

		public static string[] DetermineKeyNames( this IObjectContextAdapter target, Type type )
		{
			var entitySet = target.ObjectContext.DetermineEntitySet( type );
			var result = entitySet.With( x => x.ElementType.KeyMembers.Select( y => y.Name ).ToArray() );
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
					if ( entity.GetType().GetProperty( name ).GetCollectionType() != null )
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

		[SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract the expression." )]
		public static int Total<TEntity>( this DbContext context, Expression<Func<TEntity, bool>> predicate, EntityState? state = EntityState.Added ) where TEntity : class
		{
			var dbSet = context.Set<TEntity>();
			var result = dbSet.Count( predicate ) + dbSet.Local.Where( x =>	 context.Entry( x ).State == state ).Count( predicate.Compile() );
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
				saved.Each( x => x.Item2( x.Item1 ) );
			}
		}

		public static ObjectStateEntry GetEntry( this DbContext @this, object entity )
		{
			var objectStateManager = @this.AsTo<IObjectContextAdapter, ObjectStateManager>( x => x.ObjectContext.ObjectStateManager );
			var result = objectStateManager.GetObjectStateEntry( entity );
			return result;
		}

		public static string[] GetModifiedProperties( this DbContext @this, object entity )
		{
			var stateEntry = @this.GetEntry( entity );
			var currentValues = stateEntry.CurrentValues;
			var originalValues = stateEntry.OriginalValues;
			var result = stateEntry.GetModifiedProperties().Where( x => !originalValues.GetValue( originalValues.GetOrdinal( x ) ).Equals( currentValues.GetValue( currentValues.GetOrdinal( x ) ) ) ).ToArray();
			return result;
		}
	}
}