using System.Reflection;

namespace Prism.Modularity
{
	public interface IAssemblyProvider
	{
		Assembly[] GetAssemblies();
	}
}