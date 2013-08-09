using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.IoC.Configuration
{
    [Singleton]
	public class AssemblySupport
	{
		readonly List<Assembly> applied = new List<Assembly>();

		Action<IEnumerable<Assembly>> Action { get; set; }

		public void RegisterAndApply( Action<IEnumerable<Assembly>> action )
		{
			Action = action;
			Refresh();
		}

		void Refresh()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where( x => !x.IsDynamic ).Distinct().Except( applied ).ToArray();// moduleCatalog.Transform( x => x.Modules.Select( y => y.GetAssembly() ) ).NotNull().Union( AppDomain.CurrentDomain.GetAssemblies() ).Where( x => !x.IsDynamic ).Distinct().Except( applied ).ToArray();

			applied.AddRange(assemblies);
			Action( assemblies );
		}
	}
}