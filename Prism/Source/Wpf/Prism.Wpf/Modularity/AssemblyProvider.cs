using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Prism.Modularity
{
	public class AssemblyProvider : IAssemblyProvider
	{
		public IEnumerable<Assembly> GetAssemblies()
		{
			var result = from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
				where !( assembly is AssemblyBuilder )
					  && assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder"
					  && !string.IsNullOrEmpty( assembly.Location )
				select assembly;
			return result;
		}
	}
}