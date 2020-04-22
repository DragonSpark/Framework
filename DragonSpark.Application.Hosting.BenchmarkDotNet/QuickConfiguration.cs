using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	sealed class QuickConfiguration : ISelect<Job, IConfig>
	{
		public static QuickConfiguration Default { get; } = new QuickConfiguration();

		QuickConfiguration() {}

		public IConfig Get(Job parameter) => ManualConfig.Create(DefaultConfig.Instance)
		                                                 .AddJob(parameter)
		                                                 .AddDiagnoser(MemoryDiagnoser.Default);
	}
}