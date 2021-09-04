using DragonSpark.Application.Hosting.BenchmarkDotNet;
using DragonSpark.Compose;
using DragonSpark.Testing.Model.Selection;

namespace DragonSpark.Testing
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments).To(Run.A<SelectionTests.InParameterSelectionBenchmarks>);
		}
	}

}