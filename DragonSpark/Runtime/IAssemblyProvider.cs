using System.Reflection;

namespace DragonSpark.Runtime
{
	public interface IAssemblyProvider
	{
		Assembly[] GetAssemblies();
	}
}