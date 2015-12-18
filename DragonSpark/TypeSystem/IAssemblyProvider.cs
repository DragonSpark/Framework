using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public interface IAssemblyProvider
	{
		Assembly[] GetAssemblies();
	}
}