using System;
using System.Reflection;

namespace Prism.Modularity
{
	public abstract class AssemblyProviderBase : IAssemblyProvider
	{
		readonly Lazy<Assembly[]> all;

		protected AssemblyProviderBase()
		{
			all = new Lazy<Assembly[]>( DetermineAll );
		}

		protected abstract Assembly[] DetermineAll();
		
		public Assembly[] GetAssemblies()
		{
			var result = all.Value;
			return result;
		}
	}
}