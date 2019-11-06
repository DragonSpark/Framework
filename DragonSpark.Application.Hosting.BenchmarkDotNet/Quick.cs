using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	sealed class Quick : FixedSelectedSingleton<Job, IConfig>
	{
		public static Quick Default { get; } = new Quick();

		Quick() : base(QuickConfiguration.Default, Job.ShortRun.With(InProcessToolchain.DontLogOutput)) {}
	}
}