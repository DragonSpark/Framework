using DragonSpark.Application.Hosting.BenchmarkDotNet;
using DragonSpark.Compose;
using DragonSpark.Testing.Runtime.Execution;

namespace DragonSpark.Testing;

public class Program
{
	static void Main(params string[] arguments)
	{
		Configuration.Default.Get(arguments).To(Run.A<LogicalTests.Benchmarks>);
	}
}