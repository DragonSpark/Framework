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

		public IConfig Get(Job parameter)
		{
			var result = ManualConfig.Create(DefaultConfig.Instance);
			result.Add(parameter);
			result.Add(parameter.To(_configure));
			result.Add(MemoryDiagnoser.Default);
			return result;
		}
	}
}