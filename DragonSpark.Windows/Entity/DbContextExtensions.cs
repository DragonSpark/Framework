using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Type = System.Type;

namespace DragonSpark.Windows.Entity
{
	public static class DbContextExtensions
	{
		readonly static MethodInfo
			GetMethod = typeof(DbContextExtensions).GetMethod( nameof(DbContextExtensions.Get), new[] { typeof(DbContext), typeof(object), typeof(int) } ),
			ApplyChangesMethod = typeof(DbContextExtensions).GetMethods().FirstOrDefault( x => x.IsGenericMethod && x.Name == nameof(ApplyChanges) );

		public static int Save( this DbContext target )
		{
			try
			{
				return target.SaveChanges();
			}
			catch ( DbEntityValidationException error )
			{
				throw new EntityValidationException( target, error );
			}
		}

		public static object ApplyChanges( this DbContext target, object entity )
		{
			var type = entity.GetType();
			var items = target.GetEntityProperties( type ).SelectMany( x => type.GetProperty( x.Name ).GetValue( entity ).With( o => o.AsTo<IEnumerable, IEnumerable<object>>( enumerable => enumerable.Cast<object>().Select( target.ApplyChanges ), o.ToItem ) ) );
			
			foreach ( var o in entity.Prepend( items ).Distinct() )
			{
				ApplyChangesMethod.MakeGenericMethod( o.GetType() ).Invoke( null, new[] { target, o } );				
			}
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
			var key = new KeyFactory<TItem>( target ).Get( container );

			var current = target.Set<TItem>().Find( key.Values.ToArray() );

			var result = current.With( x => target.Include( x, levels ) );

			return result;
		}

		sealed class DefaultAssociationPropertyFactory : ParameterizedSourceBase<Type, string[]>
		{
			readonly IObjectContextAdapter adapter;

			public DefaultAssociationPropertyFactory( IObjectContextAdapter adapter )
			{
				this.adapter = adapter;
			}

			public override string[] Get( Type parameter )
			{
				var names = GetAssociationPropertyNames( adapter, parameter );
				var decorated = parameter.GetProperties().Where( x => AttributeProviderExtensions.Has<DefaultIncludeAttribute>( x ) ).Select( x => x.Name );
				var result = decorated.Union( names ).ToArray();
				return result;
			}

			static IEnumerable<string> GetAssociationPropertyNames( IObjectContextAdapter target, Type type ) => target.GetEntityProperties( type ).Select( x => type.GetProperty( x.Name ) ).Where( x => x.PropertyType.Adapt().GetInnerType() == null ).Select( x => x.Name );
		}

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

		class KeyFactory<TEntity> : ParameterizedSourceBase<IDictionary<string, object>>
		{
			readonly IObjectContextAdapter context;

			public KeyFactory( IObjectContextAdapter context )
			{
				this.context = context;
			}

			public override IDictionary<string, object> Get( object parameter )
			{
				var names = context.ObjectContext.DetermineEntitySet( typeof(TEntity) ).With( x => x.ElementType.KeyMembers.Select( y => y.Name ).ToArray() );
				return parameter.GetType().IsPrimitive ? new Dictionary<string, object> { { names.First(), parameter } } : DetermineKeyComplex( context, parameter, names );
			}

			static IDictionary<string, object> DetermineKeyComplex( IObjectContextAdapter target, object container, string[] names )
			{
				var result = names.Select( name =>
				{
					var info = container.GetType().GetProperty( name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Default | BindingFlags.FlattenHierarchy );
					var value = AttributeProviderExtensions.From<ForeignKeyAttribute, object>( typeof(TEntity).GetProperty( name ), y =>
					{
						var propertyInfo = container.GetType().GetProperty( y.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Default | BindingFlags.FlattenHierarchy );
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
		
		public static IEnumerable<NavigationProperty> GetEntityProperties( this IObjectContextAdapter target, Type type ) => target.ObjectContext.MetadataWorkspace.GetEntityMetadata( type ).NavigationProperties;

		public static Type[] GetDeclaredEntityTypes( this DbContext context ) => EnumerableExtensions.WhereAssigned( context.GetType().GetProperties().Where( x => x.PropertyType.IsGenericType && typeof( DbSet<> ).IsAssignableFrom( x.PropertyType.GetGenericTypeDefinition() ) ).Select( x => x.PropertyType.GetGenericArguments().FirstOrDefault() ) ).ToArray();

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, int levels, params Expression<Func<TEntity, object>>[] expressions ) where TEntity : class => target.Include( entity, expressions.Select( x => x.GetMemberInfo().Name ).ToArray(), levels );

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, string[] associationNames, int levels = 1 ) where TEntity : class
		{
			var associations = associationNames ?? Enumerable.Empty<string>();
			var names = associations.Union( new DefaultAssociationPropertyFactory( target ).Get( typeof(TEntity) ) ).ToArray();
			var result = Load( target, entity, names, levels );
			return result;
		}

		public static TItem Load<TItem>( this DbContext target, TItem entity, string[] properties = null, int? levels = 1, bool? loadAllProperties = null )
		{
			LoadAll( target, entity, new ArrayList(), properties, loadAllProperties.GetValueOrDefault( levels == 1 ), levels, 0 );
			return entity;
		}

		static void LoadAll( DbContext target, object entity, IList list, IEnumerable<string> properties, bool loadAllProperties, int? levels, int count )
		{
			if ( !list.Contains( entity ) )
			{
				list.Add( entity );
				var type = entity.GetType();
				var names = properties ?? ( loadAllProperties ? target.GetEntityProperties( type ).Select( x => x.Name ) : new DefaultAssociationPropertyFactory( target ).Get( type ) );
				var associationNames = names.ToArray();
				LoadEntity( target, entity, associationNames );

				if ( !levels.HasValue || ++count < levels.Value )
				{
					foreach ( var z in associationNames.Select( y => type.GetProperty( y ).GetValue( entity ) ).WhereAssigned() )
					{
						var items = z.Adapt().GetInnerType() != null ? z.AsTo<IEnumerable, object[]>( a => a.Cast<object>().ToArray() ) : z.ToItem();

						foreach ( var item in items )
						{
							LoadAll( target, item, list, null, loadAllProperties, levels, count );
						}
					}
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
	}
}