using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	sealed class Deployed : FixedSelectedSingleton<Job, IConfig>
	{
		public static Deployed Default { get; } = new Deployed();

		Deployed() : base(DeployedConfiguration.Default,
		                  Job.MediumRun.WithWarmupCount(5).WithIterationCount(10).WithLaunchCount(1)) {}
	}
}