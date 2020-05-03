using DragonSpark.Model.Results;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	class EnvironmentVariable : Instance<global::BenchmarkDotNet.Jobs.EnvironmentVariable>
	{
		public EnvironmentVariable(string name, string value)
			: this(new global::BenchmarkDotNet.Jobs.EnvironmentVariable(name, value)) {}

		public EnvironmentVariable(global::BenchmarkDotNet.Jobs.EnvironmentVariable instance) : base(instance) {}
	}

	class EnvironmentVariable<T> : EnvironmentVariable
	{
		public EnvironmentVariable(string name, T value) : this(name, value?.ToString() ?? string.Empty) {}

		public EnvironmentVariable(string name, string value) : base(name, value) {}
	}
}