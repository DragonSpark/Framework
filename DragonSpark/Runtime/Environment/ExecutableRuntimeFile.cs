using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Runtime.Environment;

sealed class ExecutableRuntimeFile : RuntimeFile
{
	public static IAlteration<string> Default { get; } = new ExecutableRuntimeFile();

	ExecutableRuntimeFile() : base(".exe") {}
}