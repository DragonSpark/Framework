namespace DragonSpark.Application.Hosting.BenchmarkDotNet;

sealed class AlignJitLoops : ConfigureJob
{
	public static AlignJitLoops Default { get; } = new();

	AlignJitLoops() : base(AlignJitLoopsSetting.Default) {}
}