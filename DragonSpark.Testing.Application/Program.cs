using DragonSpark.Application.Hosting.BenchmarkDotNet;
using DragonSpark.Compose;

namespace DragonSpark.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments).To(Run.A<IterationBenchmarks>);
		}
	}
}