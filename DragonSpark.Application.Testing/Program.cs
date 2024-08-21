using DragonSpark.Application.Testing.Entities;
using DragonSpark.Compose;

namespace DragonSpark.Application.Testing;

public class Program
{
	static void Main(params string[] arguments)
	{
		Hosting.BenchmarkDotNet.Configuration.Default.Get(arguments)
		       .To(Hosting.BenchmarkDotNet.Run.A<SaveTests.DisposeBenchmarks>);
	}
}