using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Runtime.Environment
{
	sealed class DevelopmentRuntimeFile : RuntimeFile
	{
		public static IAlteration<string> Default { get; } = new DevelopmentRuntimeFile();

		DevelopmentRuntimeFile() : base(".runtimeconfig.dev.json") {}
	}
}