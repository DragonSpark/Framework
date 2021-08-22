using DragonSpark.Application.Hosting.BenchmarkDotNet;

namespace DragonSpark.Application.Testing
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments)/*.To(Run.A<QueryBaseTests.Benchmarks>)*/;
		}
	}
}