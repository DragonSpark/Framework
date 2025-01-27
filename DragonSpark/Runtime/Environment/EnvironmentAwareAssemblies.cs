using DragonSpark.Model.Sequences;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

sealed class EnvironmentAwareAssemblies : IArray<string, Assembly>
{
	public static EnvironmentAwareAssemblies Default { get; } = new();

	EnvironmentAwareAssemblies() {}

	public Array<Assembly> Get(string parameter)
	{
		var names    = new ComponentAssemblyNames(parameter);
		var selector = new AssemblySelector(names);
		var result   = new Assemblies(selector).Get();
		return result;
	}
}