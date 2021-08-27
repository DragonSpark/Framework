using DragonSpark.Application.Hosting.BenchmarkDotNet;
using DragonSpark.Application.Testing.Compose.Entities.Queries;
using DragonSpark.Compose;

namespace DragonSpark.Application.Testing
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments).To(Run.A<FormAdapterTests.Benchmarks>);
		}
	}
}