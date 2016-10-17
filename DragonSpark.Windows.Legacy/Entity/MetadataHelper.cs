using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;

namespace DragonSpark.Windows.Legacy.Entity
{
	public static class MetadataHelper
	{
		readonly static Dictionary<Type, EntityType> EntityTypes = new Dictionary<Type, EntityType>();

		/// <summary>
		/// Gets the entity meta data.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context">The context.</param>
		/// <returns>EntityType.</returns>
		public static EntityType GetEntityMetadata<T>( this ObjectContext context ) => GetEntityMetadata( context.MetadataWorkspace, typeof(T) );

		/// <summary>
		/// Gets the entity meta data.
		/// </summary>
		/// <param name="workspace">The workspace.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns>EntityType.</returns>
		public static EntityType GetEntityMetadata( this MetadataWorkspace workspace, Type entityType )
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

		/// <summary>
		/// Gets the hierarchy.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>IEnumerable&lt;EntityType&gt;.</returns>
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

		/// <summary>
		/// Creates the key.
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <typeparam name="TId"></typeparam>
		/// <param name="context">The context.</param>
		/// <param name="id">The identifier.</param>
		/// <returns>EntityKey.</returns>
		public static EntityKey CreateKey<TContext, TId>( ObjectContext context, TId id ) where TContext : EntityObject where TId : struct
		{
			var type = GetEntityMetadata<TContext>( context );
			var list = new List<EntityKeyMember> { new EntityKeyMember( new List<EdmMember>( type.KeyMembers )[ 0 ].Name, id ) };
			return CreateKey( context, typeof(TContext), list );
		}

		/// <summary>
		/// Creates the key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context">The context.</param>
		/// <param name="values">The values.</param>
		/// <returns>EntityKey.</returns>
		public static EntityKey CreateKey<T>( ObjectContext context, params object[] values )
		{
			var type = GetEntityMetadata( context.MetadataWorkspace, typeof(T) );
			var list = new List<EntityKeyMember>( type.KeyMembers.Select( x => new EntityKeyMember( x.Name, values[ type.KeyMembers.IndexOf( x ) ] ) ) );
			return CreateKey( context, typeof(T), list );
		}

		/// <summary>
		/// Creates the key.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="pairs">The pairs.</param>
		/// <returns>EntityKey.</returns>
		public static EntityKey CreateKey( ObjectContext context, Type entityType,
			IEnumerable<EntityKeyMember> pairs )
		{
			var name = string.Concat( context.DefaultContainerName, ".", context.DetermineEntitySet( entityType ) );
			var result = new EntityKey( name, pairs );
			return result;
		}

		/// <summary>
		/// Extracts the key.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="target">The target.</param>
		/// <returns>EntityKey.</returns>
		public static EntityKey ExtractKey( this ObjectContext context, object target ) => ExtractKey( context, target.GetType(), target );

		/// <summary>
		/// Extracts the key.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="type">The type.</param>
		/// <param name="target">The target.</param>
		/// <returns>EntityKey.</returns>
		public static EntityKey ExtractKey( ObjectContext context, Type type, object target )
		{
			var metadata = GetEntityMetadata( context.Initialized().MetadataWorkspace, type );
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

		/// <summary>
		/// Extracts the key values.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="target">The target.</param>
		/// <returns>List&lt;EntityKeyMember&gt;.</returns>
		public static IEnumerable<EntityKeyMember> ExtractKeyValues( EntityType type, object target )
		{
			var dictionary = target as IDictionary;
			var targetType = target.GetType();
			return ( from property in ConvertProperties<EdmProperty, EdmMember>( type.KeyMembers ) let value = dictionary != null && dictionary.Contains( property.Name ) ? dictionary[property.Name] : targetType.GetProperty( property.Name ).GetValue( target, null ) select new EntityKeyMember( property.Name, value ) ).ToList();
		}

		/// <summary>
		/// Converts the properties.
		/// </summary>
		/// <typeparam name="TTo"></typeparam>
		/// <typeparam name="TFrom"></typeparam>
		/// <param name="members">The members.</param>
		/// <returns>IEnumerable&lt;T&gt;.</returns>
		internal static IEnumerable<TTo> ConvertProperties<TTo, TFrom>( IEnumerable<TFrom> members ) where TTo : MetadataItem
			where TFrom : MetadataItem
		{
			var result = from member in members.OfType<TTo>()
				select member;
			return result;
		}

		/// <summary>
		/// Converts the properties.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="members">The members.</param>
		/// <returns>IEnumerable&lt;T&gt;.</returns>
		internal static IEnumerable<T> ConvertProperties<T>( IEnumerable<MetadataItem> members ) where T : MetadataItem
		{
			return ConvertProperties<T, MetadataItem>( members );
		}
	}
}