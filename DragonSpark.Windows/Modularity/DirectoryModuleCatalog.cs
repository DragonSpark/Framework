using DragonSpark.Modularity;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;

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
		readonly IFactory<LoadRemoteModuleInfoParameter, ModuleInfo[]> factory;

		public DirectoryModuleCatalog() : this( FileSystemAssemblyProvider.Instance, ModuleInfoBuilder.Instance, LoadRemoteModuleInfoFactory.Instance )
		{}

		public DirectoryModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder, IFactory<LoadRemoteModuleInfoParameter, ModuleInfo[]> factory ) : base( provider, builder )
		{
			this.factory = factory;
			ModulePath = ".";
		}

		/// <summary>
		/// Directory containing modules to search for.
		/// </summary>
		public string ModulePath { get; set; }

		protected override IEnumerable<ModuleInfo> GetModuleInfos( IEnumerable<Assembly> assemblies )
		{
			var parameter = new LoadRemoteModuleInfoParameter( assemblies.Select( assembly => assembly.Location ), ModulePath );
			var result = factory.Create( parameter );
			return result;
		}
	}

	public class LoadRemoteModuleInfoFactory : FactoryBase<LoadRemoteModuleInfoParameter, ModuleInfo[]>
	{
		public static LoadRemoteModuleInfoFactory Instance { get; } = new LoadRemoteModuleInfoFactory();

		readonly IFactory<LoadRemoteModuleInfoParameter, IModuleInfoProvider> factory;

		public LoadRemoteModuleInfoFactory() : this( RemoteModuleInfoProviderFactory.Instance )
		{}

		public LoadRemoteModuleInfoFactory( IFactory<LoadRemoteModuleInfoParameter, IModuleInfoProvider> factory )
		{
			this.factory = factory;
		}

		protected override ModuleInfo[] CreateItem( LoadRemoteModuleInfoParameter parameter )
		{
			using ( var loader = factory.Create( parameter ) )
			{
				var result = loader.GetModuleInfos().Fixed();
				return result;
			}
		}
	}

	public class LoadRemoteModuleInfoParameter
	{
		public LoadRemoteModuleInfoParameter( IEnumerable<string> assemblyLocations, string path )
		{
			Locations = assemblyLocations.Fixed();
			Path = path;
		}

		public string[] Locations { get; }
		public string Path { get; }
	}

	public class RemoteModuleInfoProviderFactory : FactoryBase<LoadRemoteModuleInfoParameter, IModuleInfoProvider>
	{
		public static RemoteModuleInfoProviderFactory Instance { get; } = new RemoteModuleInfoProviderFactory();

		readonly IModuleInfoBuilder builder;
		readonly IFactory<AppDomain, AppDomain> factory;

		public RemoteModuleInfoProviderFactory() : this( ModuleInfoBuilder.Instance, ChildDomainFactory.Instance )
		{}

		public RemoteModuleInfoProviderFactory( IModuleInfoBuilder builder, IFactory<AppDomain, AppDomain> factory )
		{
			this.builder = builder;
			this.factory = factory;
		}

		protected override IModuleInfoProvider CreateItem( LoadRemoteModuleInfoParameter parameter )
		{
			var loaded = parameter.Locations.ToArray();
			var child = factory.Create( AppDomain.CurrentDomain );
			var result = new RemotingModuleInfoProvider<DirectoryModuleInfoProvider>( child, builder, loaded, parameter.Path );
			return result;
		}
	}

	public class ChildDomainFactory : FactoryBase<AppDomain, AppDomain>
	{
		public static ChildDomainFactory Instance { get; } = new ChildDomainFactory();

		readonly string name;

		public ChildDomainFactory( string name = "DiscoveryRegion" )
		{
			this.name = name;
		}

		protected override AppDomain CreateItem( AppDomain parameter )
		{
			var evidence = new Evidence(parameter.Evidence);
			var setup = parameter.SetupInformation;
			var result = AppDomain.CreateDomain( name, evidence, setup );
			return result;
		}
	}
}