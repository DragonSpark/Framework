namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	sealed class AlignJitLoopsSetting : EnvironmentVariable<int>
	{
		public static AlignJitLoopsSetting Default { get; } = new AlignJitLoopsSetting();

		AlignJitLoopsSetting() : base("COMPlus_JitAlignLoops", 1) {}
	}
}