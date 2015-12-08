using DragonSpark.Modularity;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;

namespace DragonSpark.Windows.Modularity
{
	/// <summary>
	/// Represets a catalog created from a directory on disk.
	/// </summary>
	/// <remarks>
	/// The directory catalog will scan the contents of a directory, locating classes that implement
	/// <see cref="IModule"/> and add them to the catalog based on contents in their associated <see cref="ModuleAttribute"/>.
	/// Assemblies are loaded into a new application domain with ReflectionOnlyLoad.  The application domain is destroyed
	/// once the assemblies have been discovered.
	/// 
	/// The diretory catalog does not continue to monitor the directory after it has created the initialze catalog.
	/// </remarks>
	public class DirectoryModuleCatalog : AssemblyModuleCatalog
	{
		public DirectoryModuleCatalog() : this( FileSystemAssemblyProvider.Instance, new DynamicModuleInfoBuilder() )
		{}

		public DirectoryModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder ) : base( provider, builder )
		{
			ModulePath = ".";
		}

		/// <summary>
		/// Directory containing modules to search for.
		/// </summary>
		public string ModulePath { get; set; }

		protected override IEnumerable<ModuleInfo> GetModuleInfos( IEnumerable<Assembly> assemblies )
		{
			var loaded = assemblies.Select( assembly => assembly.Location ).ToArray();

			using ( var loader = Create( loaded ) )
			{
				var result = loader.GetModuleInfos();
				return result;
			}
		}

		protected virtual IModuleInfoProvider Create( IEnumerable<string> assemblies )
		{
			AppDomain childDomain = this.BuildChildDomain(AppDomain.CurrentDomain);
			var result = new RemotingModuleInfoProvider<DirectoryModuleInfoProvider>( childDomain, Builder, assemblies, ModulePath );
			return result;
		}

			
		/// <summary>
		/// Creates a new child domain and copies the evidence from a parent domain.
		/// </summary>
		/// <param name="parentDomain">The parent domain.</param>
		/// <returns>The new child domain.</returns>
		/// <remarks>
		/// Grabs the <paramref name="parentDomain"/> evidence and uses it to construct the new
		/// <see cref="AppDomain"/> because in a ClickOnce execution environment, creating an
		/// <see cref="AppDomain"/> will by default pick up the partial trust environment of 
		/// the AppLaunch.exe, which was the root executable. The AppLaunch.exe does a 
		/// create domain and applies the evidence from the ClickOnce manifests to 
		/// create the domain that the application is actually executing in. This will 
		/// need to be Full Trust for Prism applications.
		/// </remarks>
		/// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="parentDomain"/> is null.</exception>
		protected virtual AppDomain BuildChildDomain(AppDomain parentDomain)
		{
			if (parentDomain == null) throw new ArgumentNullException(nameof( parentDomain ));

			Evidence evidence = new Evidence(parentDomain.Evidence);
			AppDomainSetup setup = parentDomain.SetupInformation;
			return AppDomain.CreateDomain("DiscoveryRegion", evidence, setup);
		}
	}
}