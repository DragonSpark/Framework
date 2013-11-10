using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Resources.Tools;
using System.Text;
using DragonSpark.Extensions;

namespace DragonSpark.Serialization
{
	public class TypeResourceStreamResolver : IStreamResolver
	{
		readonly string resourceName;
		readonly Type resourceType;
		static readonly Dictionary<Assembly, Type> cache = new Dictionary<Assembly, Type>();

		public TypeResourceStreamResolver( Type resourceType, string resourceName )
		{
			Contract.Requires( !string.IsNullOrEmpty( resourceName ) );
			this.resourceName = resourceName;
			this.resourceType = resourceType ?? ResolveDefaultResourceType();
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( !string.IsNullOrEmpty( resourceName ) );
		}*/

		static Type ResolveDefaultResourceType()
		{
			var key = Assembly.GetCallingAssembly();
			if ( !cache.ContainsKey( key ) )
			{
				cache[ key ] = ResolveType( key );
			}
			return cache[ key ];
		}

		static Type ResolveType( Assembly key )
		{
			foreach ( var type in key.GetValidTypes() )
			{
				var attribute = Attribute.GetCustomAttribute( type, typeof(GeneratedCodeAttribute) ) as GeneratedCodeAttribute;
				if ( attribute != null && attribute.Tool == typeof(StronglyTypedResourceBuilder).FullName )
				{
					return type;
				}
			}
			throw new InvalidOperationException( string.Format( "No default resource type found for assembly {0}", key.FullName ) );
		}

		public Type ResourceType
		{
			get { return resourceType; }
		}

		public string ResourceName
		{
			get { return resourceName; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Stream is disposed elsewhere.")]
		public Stream ResolveStream()
		{
			var source = ResourceStringLoader.LoadString( ResourceType.FullName, ResourceName, ResourceType.Assembly );
			if ( !string.IsNullOrEmpty( source ) )
			{
				return new MemoryStream( Encoding.Default.GetBytes( source ) );
			}
			throw new InvalidOperationException( string.Format( "Could not find resource '{0}' in type '{1}'.", ResourceName,
			                                                    ResourceType ) );
		}

		static class ResourceStringLoader
		{
			// Methods
			static string LoadAssemblyString( Assembly asm, string baseName, string resourceName )
			{
				try
				{
					var manager = new ResourceManager( baseName, asm );
					return manager.GetString( resourceName );
				}
				catch ( MissingManifestResourceException )
				{}
				return null;
			}

			public static string LoadString( string baseName, string resourceName, Assembly asm )
			{
				if ( string.IsNullOrEmpty( baseName ) )
				{
					throw new ArgumentNullException( "baseName" );
				}
				if ( string.IsNullOrEmpty( resourceName ) )
				{
					throw new ArgumentNullException( "resourceName" );
				}
				string str = null;
				if ( asm != null )
				{
					str = SearchForResource( asm, resourceName );
				}
				if ( str == null )
				{
					str = LoadAssemblyString( Assembly.GetExecutingAssembly(), baseName, resourceName );
				}
				if ( str == null )
				{
					return string.Empty;
				}
				return str;
			}

			static string SearchForResource( Assembly asm, string resourceName )
			{
				foreach ( var str in asm.GetManifestResourceNames() )
				{
					var str2 = (string)str.Clone();
					if ( str.EndsWith( ".resources" ) )
					{
						str2 = str.Replace( ".resources", string.Empty );
					}
					var str3 = LoadAssemblyString( asm, str2, resourceName );
					if ( !string.IsNullOrEmpty( str3 ) )
					{
						return str3;
					}
				}
				return null;
			}
		}
	}
}