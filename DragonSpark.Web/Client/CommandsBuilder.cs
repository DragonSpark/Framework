using DragonSpark.Extensions;
using DragonSpark.Io;
using DragonSpark.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace DragonSpark.Web.Client
{
	public class ResourcesContainer : Dictionary<string, IDictionary<string,string>>
	{}

	public class ResourcesContainerBuilder : Factory<ResourcesContainer>
	{
		readonly string resourcesNamespace;
		readonly Assembly applicationAssembly;

		public ResourcesContainerBuilder( Assembly applicationAssembly = null, string resourcesNamespace = "Resources" )
		{
			this.resourcesNamespace = resourcesNamespace;
			this.applicationAssembly = applicationAssembly ?? BuildManager.GetGlobalAsaxType().BaseType.Assembly;
		}

		protected override ResourcesContainer CreateItem( object parameter )
		{
			var result = new ResourcesContainer();
			applicationAssembly.GetTypes().Where( x => x.Namespace == resourcesNamespace ).Apply( x =>
			{
				var propertyInfos = x.GetProperties( DragonSparkBindingOptions.AllProperties ).Where( y => y.PropertyType == typeof(string) );
				result.Add( x.Name, propertyInfos.ToDictionary( y =>  y.Name, y => (string)y.GetValue( null, null ) ) );
			} );
			return result;
		}
	}

	public class CommandsBuilder : ClientModuleBuilder
	{
		public CommandsBuilder( IPathResolver pathResolver, string initialPath = "commands" ) : base( pathResolver, initialPath )
		{}

		protected override IEnumerable<ClientModule> Create( FileInfo entryPoint, DirectoryInfo root )
		{
			var result = new DirectoryInfo( Path.Combine( entryPoint.DirectoryName, InitialPath ) ).EnumerateFiles( "*.js", SearchOption.AllDirectories ).Select( x => new ClientModule
				{
					Path = root.DetermineRelative( x, false ).Transform( z => Path.Combine( Path.GetDirectoryName( z ), Path.GetFileNameWithoutExtension(z) ).ToUri() )
				} ).ToArray();
			return result;
		}
	}

	public class InitializersBuilder : ClientModuleBuilder
	{
		public InitializersBuilder( IPathResolver pathResolver, string initialPath = "viewmodels" ) : base( pathResolver, initialPath )
		{}

		protected override IEnumerable<ClientModule> Create( FileInfo entryPoint, DirectoryInfo root )
		{
			var result = new DirectoryInfo( Path.Combine( entryPoint.DirectoryName, InitialPath ) ).EnumerateFiles( "*.initialize.js", SearchOption.AllDirectories ).Select( x => new ClientModule
				{
					Path = root.DetermineRelative( x, false ).Transform( z => Path.Combine( Path.GetDirectoryName( z ), Path.GetFileNameWithoutExtension(z) ).ToUri() )
				} ).ToArray();
			return result;
		}
	}
}