using DragonSpark.Extensions;
using DragonSpark.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Server.ClientHosting
{
	public class AssemblyResource
	{
		readonly Assembly assembly;
		readonly string resourceName;

		public AssemblyResource( Assembly assembly, string resourceName )
		{
			this.assembly = assembly;
			this.resourceName = resourceName;
		}

		public Assembly Assembly
		{
			get { return assembly; }
		}

		public string ResourceName
		{
			get { return resourceName; }
		}
	}

	public abstract class ClientModuleBuilder : ClientModuleBuilder<ClientModule>
	{
		protected ClientModuleBuilder( string initialPath ) : base( initialPath )
		{}

		protected override ClientModule Create( AssemblyResource resource )
		{
			return new ClientModule
			{
				Path = string.Concat( resource.Assembly.FromMetadata<ClientResourcesAttribute, string>( x => x.Name ), Path.AltDirectorySeparatorChar, Path.GetFileNameWithoutExtension( resource.ResourceName ) )
			};
		}
	}

	public abstract class ClientModuleBuilder<TModule> : Factory<string, IEnumerable<TModule>> where TModule : ClientModule
	{
		readonly string initialPath;

		protected ClientModuleBuilder( string initialPath )
		{
			this.initialPath = initialPath;
		}

		public string InitialPath
		{
			get { return initialPath; }
		}

		protected override IEnumerable<TModule> CreateItem( string parameter )
		{
			var result = AppDomain.CurrentDomain.GetAssemblies()
				.Where( x => x.IsDecoratedWith<ClientResourcesAttribute>() )
				.SelectMany( x => x.GetManifestResourceNames().Select( y => new AssemblyResource( x, y ) ) ).Where( IsResource ).Select( Create ).ToArray();
			return result;
		}

		protected virtual bool IsResource( AssemblyResource resource )
		{
			var result = resource.ResourceName.StartsWith( string.Concat( resource.Assembly.GetName().Name, ".", InitialPath ), StringComparison.InvariantCultureIgnoreCase );
			return result;
		}

		protected abstract TModule Create( AssemblyResource resource );
	}
}