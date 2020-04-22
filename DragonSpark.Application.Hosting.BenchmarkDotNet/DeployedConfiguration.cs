using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	sealed class DeployedConfiguration : ISelect<Job, IConfig>
	{
		public static DeployedConfiguration Default { get; } = new DeployedConfiguration();

		DeployedConfiguration() : this(AlignJitLoops.Default) {}

		readonly IAlteration<Job> _configure;

		public DeployedConfiguration(IAlteration<Job> configure) => _configure = configure;

		public IConfig Get(Job parameter) => ManualConfig.Create(DefaultConfig.Instance)
		                                                 .AddJob(parameter)
		                                                 .AddJob(parameter.To(_configure))
		                                                 .AddDiagnoser(MemoryDiagnoser.Default);
	}
}