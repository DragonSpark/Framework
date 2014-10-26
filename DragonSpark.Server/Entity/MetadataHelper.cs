using DragonSpark.ComponentModel;
using DragonSpark.IoC.Commands;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web.Hosting;
using System.Windows.Markup;

namespace DragonSpark.Server.Entity
{
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

	[ContentProperty( "Initializer" )]
	public class PreloadApplicationCommand : IContainerConfigurationCommand
	{
		[DefaultPropertyValue( true )]
		public bool IsEnabled { get; set; }

		public Type PreloaderType { get; set; }

		public void Configure( IUnityContainer container )
		{
			IsEnabled.IsTrue( () =>
			{
				var client = PreloaderType.Transform( Activator.CreateInstance<IProcessHostPreloadClient> );
				client.Preload( new string[0] );
			} );
		}
	}
}