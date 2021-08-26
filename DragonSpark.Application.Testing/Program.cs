using DragonSpark.Application.Hosting.BenchmarkDotNet;
using DragonSpark.Application.Testing.Entities.Queries;
using DragonSpark.Compose;

namespace DragonSpark.Application.Testing
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments).To(Run.A<QueryTests.Benchmarks>);
		}
	}
}