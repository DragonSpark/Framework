using System.Reflection;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyLoader : ConfigurableParameterizedSource<string, Assembly>
	{
		public static AssemblyLoader Default { get; } = new AssemblyLoader();
		AssemblyLoader() {}
	}
}