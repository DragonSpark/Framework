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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Entity
{
	public interface ITransactionProvider
	{
		DbContextTransaction Create();
	}

	[Registration( typeof(TransientLifetimeManager) )]
	class TransactionProvider : ITransactionProvider
	{
		readonly DbContext context;

		public TransactionProvider( DbContext context )
		{
			this.context = context;
		}

		public DbContextTransaction Create()
		{
			var result = context.Database.BeginTransaction();
			return result;
		}
	}

	public class TransactionScopeInterceptionBehavior : InterceptionBehaviorBase
	{
		readonly ITransactionProvider provider;

		public TransactionScopeInterceptionBehavior( ITransactionProvider provider )
		{
			this.provider = provider;
		}

		protected override IMethodReturn Invoke( IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext )
		{
			var candidates = new[] { input.MethodBase, input.Target.GetType().GetMethod( input.MethodBase.Name, input.MethodBase.GetParameters().Select( x => x.ParameterType ).ToArray() ) }.NotNull().ToArray();
			var scope = candidates.Any( x => x.IsDecoratedWith<ApplyTransactionAttribute>() ) ? provider.Create() : null;
			var result = getNext()( input, getNext );
			scope.NotNull( x =>
			{
				var action = result.Exception != null ? (Action)x.Rollback : x.Commit;
				action();
			} );

			return result;
		}
	}

	public class EntityValidationException : Exception
	{
		public EntityValidationException( DbContext context, DbEntityValidationException innerException ) : base( DetermineMessage( context, innerException ), innerException )
		{}

		static string DetermineMessage( DbContext context, DbEntityValidationException error )
		{
			var result = string.Concat( "Validation Exceptions were encountered while commiting entities to storage.  These are the exceptions:", Environment.NewLine, 
				string.Join( string.Empty, error.EntityValidationErrors.Select( x => string.Format( "-= {0} [State: {1}] [Key: {2}] =-{3}{4}", x.Entry.Entity.GetType(), x.Entry.State, DetermineKey( context, x ), Environment.NewLine, string.Join( Environment.NewLine, x.ValidationErrors.Select( y => string.Concat( "	- ", y.ErrorMessage ) ).ToArray() ) ) ) ) );
			return result;
		}

		static string DetermineKey( DbContext context, DbEntityValidationResult validationResult )
		{
			var key = MetadataHelper.ExtractKey( context.AsTo<IObjectContextAdapter, ObjectContext>( y => y.ObjectContext ), validationResult.Entry.Entity );
			var result = string.Join( ", ", key.EntityKeyValues.Select( x => x.ToString() ) );
			return result;
		}
	}

	public static class DbSetExtensions
	{
		public static TEntity Find<TEntity>( this IDbSet<TEntity> @this, Expression<Func<TEntity, bool>> where ) where TEntity : class
		{
			var result = @this.Local.FirstOrDefault( where.Compile() ) ?? @this.FirstOrDefault( where );
			return result;
		}
	}

	public static class Entities
	{
		public static void Migrate<TContext, TConfiguration>( Action<TContext> initializing = null, Action<TContext> initialized = null ) where TContext : DbContext, new() where TConfiguration : DbMigrationsConfiguration<TContext>, new()
		{
			Initialize<TContext, MigrateDatabaseToLatestVersion<TContext, TConfiguration>>( initializing, initialized );
		}

		public static void Initialize<TContext, TInitializer>( Action<TContext> initializing = null, Action<TContext> initialized = null ) where TContext : DbContext, new() where TInitializer : IDatabaseInitializer<TContext>, new()
		{
			Database.SetInitializer( new TInitializer() );
			using ( var context = new TContext() )
			{
				initializing.NotNull( x => x( context ) );
				context.Database.Initialize( true );
				initialized.NotNull( x => x( context ) );
			}
		}
	}

	public static class DbContextExtensions
	{
		static readonly MethodInfo
			GetMethod = typeof(DbContextExtensions).GetMethod( "Get", new[] { typeof(DbContext), typeof(object), typeof(int) } ),
			ApplyChangesMethod = typeof(DbContextExtensions).GetMethods().FirstOrDefault( x => x.IsGenericMethod && x.Name == "ApplyChanges" );

		public static int Save( this DbContext target, bool? validate = null, bool? autoDetectChanges = null )
		{
			try
			{
				using ( target.Configured( x => { x.ValidateOnSaveEnabled = validate.HasValue ? validate.Value : x.ValidateOnSaveEnabled; x.AutoDetectChangesEnabled = autoDetectChanges.HasValue ? autoDetectChanges.Value : x.AutoDetectChangesEnabled; } ) )
				{
					return target.SaveChanges();
				}
			}
			catch ( DbEntityValidationException error )
			{
				throw new EntityValidationException( target, error );
			}
		}

		public static object ApplyChanges( this DbContext target, object entity )
		{
			var result = ApplyChangesMethod.MakeGenericMethod( entity.GetType() ).Invoke( null, new[] { target, entity } );
			return result;
		}

		public static TEntity ApplyChanges<TEntity>( this DbContext target, TEntity entity ) where TEntity : class
		{
			switch ( target.Entry( entity ).State )
			{
				case EntityState.Detached:
					target.Set<TEntity>().AddOrUpdate( entity );
					switch ( target.Entry( entity ).State )
					{
						case EntityState.Detached:
							return target.Get<TEntity>( entity );
					}
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
				
				var result = current.Transform( x => target.Include( x, levels ) );

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

			public ObjectContext ObjectContext
			{
				get { return objectContext ?? ( objectContext = DetermineContext() ); }
			}

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
			var decorated = type.GetProperties().Where( x => x.IsDecoratedWith<DefaultIncludeAttribute>() ).SelectMany( x => includeOtherPath ? x.FromMetadata<DefaultIncludeAttribute, IEnumerable<string>>( y => y.AlsoInclude == "*" ? DetermineDefaultAssociationPaths( target, x.PropertyType, false ) : y.AlsoInclude.ToStringArray() ).Select( z => string.Concat( x.Name, ".", z ) ).Transform( a => a.Any() ? a : x.Name.Append() ) : x.Name.Append() ).ToArray();
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
			properties.Apply( x =>
			{
				var property = typeof(TItem).GetProperty( x );
				var current = property.GetValue( item );
				current.NotNull( y =>
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

				properties.Apply( x =>
				{
					var property = type.GetProperty( x );
					var raw = property.GetValue( entity );
					var items = property.GetCollectionType() != null ? raw.To<IEnumerable>().Cast<object>().ToArray() : new[] { raw };
					items.Apply( y => context.Set( y.GetType() ).Remove( y ) );
					context.Save();
				} );
			}

			context.Set<T>().Remove( entity );
		}

		public static object[] ResolveKeyValues( this DbContext target, object entity )
		{
			var type = entity.GetType();
			var propertyInfos = type.GetProperties().Where( x => x.IsDecoratedWith<KeyAttribute>() ).OrderBy( x => x.FromMetadata<DisplayAttribute, int>( y => y.GetOrder().GetValueOrDefault( 0 ), () => 0 ) );
			var result = propertyInfos.Select( x => type.GetProperty( x.Name ).GetValue( entity, null ) ).ToArray();
			return result;
		}

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
					return o.Transform( z =>
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
						var items = TypeExtensions.GetItemType( z.GetType() ) != null ? z.AsTo<IEnumerable, object[]>( a => a.Cast<object>().ToArray() ) : z.Append();
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
				saved.Apply( x => x.Item2( x.Item1 ) );
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
}