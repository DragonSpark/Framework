using System.Collections.Generic;
using System.Reflection;

namespace Prism.Modularity
{
	public interface IAssemblyProvider
	{
		IEnumerable<Assembly> GetAssemblies();
	}
}