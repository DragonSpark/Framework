using DragonSpark.Sources.Scopes;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyLoader : ParameterizedScope<string, Assembly>
	{
		public static AssemblyLoader Default { get; } = new AssemblyLoader();
		AssemblyLoader() {}
	}
}