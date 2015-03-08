using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application.Communication.Entity
{
	public static class MetadataHelper
	{
		static readonly Dictionary<Type, EntityType> entityTypes = new Dictionary<Type, EntityType>();

		public static EntityType GetEntityMetaData<T>( ObjectContext context ) where T : EntityObject
		{
			return GetEntityMetaData( context.MetadataWorkspace, typeof(T) );
		}

		public static EntityType GetEntityMetaData( MetadataWorkspace workspace, Type entityType )
		{
			if ( !entityTypes.ContainsKey( entityType ) )
			{
				var type = ResolveType( workspace, entityType );
				entityTypes.Add( entityType, type );
			}
			return entityTypes[ entityType ];
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
				var item = (EntityType)workspace.GetType( type.Name, namespaceName, true, DataSpace.CSpace );
				return item;
			}
		}

		public static EntityKey CreateKey<T, U>( ObjectContext context, U id ) where T : EntityObject where U : struct
		{
			var type = GetEntityMetaData<T>( context );
			var list = new List<EntityKeyMember> { new EntityKeyMember( new List<EdmMember>( type.KeyMembers )[ 0 ].Name, id ) };
			return CreateKey( context, typeof(T), list );
		}

		public static EntityKey CreateKey( ObjectContext context, Type entityType,
		                                   IEnumerable<EntityKeyMember> pairs )
		{
			var name = string.Concat( context.DefaultContainerName, ".", context.ResolveEntitySet( entityType ) );
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
			return ( ConvertProperties<EdmProperty, EdmMember>( type.KeyMembers ).Select( property => new { property, value = dictionary != null && dictionary.Contains( property.Name ) ? dictionary[property.Name] : targetType.GetProperty( property.Name ).GetValue( target, null ) } ).Select( @t => new EntityKeyMember( @t.property.Name, @t.value ) ) ).ToList();
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

	public static partial class PropertyInfoExtensions
	{
		public static bool IsKey( this PropertyInfo target )
		{
			var result = target.FromMetadata<EdmScalarPropertyAttribute, bool>( item => item.EntityKeyProperty );
			return result;
		}
	}

	public static class EntityExtensions
	{
		static readonly Dictionary<Type,PropertyInfo[]> Cache = new Dictionary<Type, PropertyInfo[]>();

		public static IEnumerable<PropertyInfo> ResolveKeyProperties( this EntityObject target )
		{
			Contract.Requires( target != null );

			var result = Cache.Ensure( target.GetType(), ResolveProperties );
			return result;
		}

		static PropertyInfo[] ResolveProperties( Type type )
		{
			var query = from property in type.GetProperties()
			            where property.IsKey()
			            select property;
			var result = query.ToArray();
			return result;
		}


		public static TEnd EnsureLoaded<TEnd>( this EntityObject target, TEnd reference ) where TEnd : IRelatedEnd
		{
			if ( target.EntityKey != null )
			{
				switch ( target.EntityState )
				{
					case EntityState.Modified:
					case EntityState.Unchanged:
						if ( !reference.IsLoaded )
						{
							try
							{
								reference.Load();
							}
							catch ( InvalidOperationException )
							{}
						}
						break;
				}
			}
			return reference;
		}
	}

	public static class ObjectQueryExtensions
	{
		public static ObjectQuery<TEntity> IncludeAll<TEntity>( this ObjectQuery<TEntity> target )
		{
			var query = from property in typeof(TEntity).GetProperties( DragonSparkBindingOptions.AllProperties )
			            where typeof(EntityObject).IsAssignableFrom( property.PropertyType )
			            select property.Name;
			foreach ( var include in query )
			{
				target = target.Include( include );
			}
			return target;
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
			                            target.ResolveEntitySet( entityType ).Name, where );
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


		public static EntitySet ResolveEntitySet<TEntity>( this ObjectContext context )
		{
			var result = context.ResolveEntitySet( typeof(TEntity) );
			return result;
		}

        public static EntitySet ResolveEntitySet( this ObjectContext context, Type entityType )
		{
			var query = from property in context.GetType().GetProperties()
			            where property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObjectSet<>)
						where property.PropertyType.GetGenericArguments().First().IsAssignableFrom( entityType )
						let container = context.MetadataWorkspace.GetEntityContainer( context.DefaultContainerName, DataSpace.CSpace )
						select container.GetEntitySetByName( property.Name, true );
			var result = query.Single();
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
			var entitySet = target.ResolveEntitySet( entity.GetType() );
			target.AddObject( entitySet.Name, entity );
		}

		public static IQueryable GetQuery( this ObjectContext target, Type entityType )
		{
			var entitySet = target.ResolveEntitySet( entityType );
			var query = target.GetType().GetProperty( entitySet.Name ).GetValue( target, null );
			var result = query.GetType().GetMethod( "OfType" ).MakeGenericMethod( entityType ).Invoke( query, null ).As<IQueryable>();
			return result;
		}

		public static ObjectQuery<TEntity> GetQuery<TEntity>( this ObjectContext target )
		{
			var entitySet = target.ResolveEntitySet<TEntity>();
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
}