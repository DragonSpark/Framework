using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Modularity;

namespace DragonSpark.IoC.Configuration
{
    [Singleton]
	public class AssemblySupport
	{
		readonly IModuleCatalog moduleCatalog;
		readonly Collection<Assembly> applied = new Collection<Assembly>();

		public AssemblySupport( IModuleManager moduleManager, IModuleCatalog moduleCatalog )
		{
			this.moduleCatalog = moduleCatalog;
			moduleManager.LoadModuleCompleted += ( s, a ) => a.Error.Null( Refresh );
		}

		Action<Assembly> Action { get; set; }

		public void RegisterAndApply( Action<Assembly> action )
		{
			Action = action;
			Refresh();
		}

		void Refresh()
		{
			var assemblies = moduleCatalog.Transform( x => x.Modules.Select( y => y.GetAssembly() ) ).NotNull().Union( AppDomain.CurrentDomain.GetAssemblies() ).Where( x => !x.IsDynamic ).Distinct().Except( applied ).ToArray();
			assemblies.Apply( x =>
			{
				applied.Add( x );
				Action( x );
			} );
		}
	}
}