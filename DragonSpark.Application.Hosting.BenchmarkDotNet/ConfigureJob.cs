using BenchmarkDotNet.Jobs;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using System.Linq;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	class ConfigureJob : IAlteration<Job>
	{
		readonly Array<global::BenchmarkDotNet.Jobs.EnvironmentVariable> _variables;

		public ConfigureJob(params global::BenchmarkDotNet.Jobs.EnvironmentVariable[] variables)
			=> _variables = variables;

		public Job Get(Job parameter) => _variables.Open().ToArray().To(parameter.WithEnvironmentVariables);
	}
}