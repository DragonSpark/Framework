using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using System.Threading.Tasks;

namespace DragonSpark.Testing.Runtime.Execution
{
	public sealed class LogicalTests
	{
		public class Benchmarks
		{
			readonly Logical<object> _subject;

			public Benchmarks() : this(new Logical<object>()) {}

			public Benchmarks(Logical<object> subject) => _subject = subject;

			[GlobalSetup]
			public Task GlobalSetup()
			{
				_subject.Execute(new object());
				return Task.CompletedTask;
			}

			[Benchmark]
			public ValueTask<object?> Measure()
			{
				var result = _subject.Get();
				return result.ToOperation();
			}

			[Benchmark]
			public void Assign()
			{

			}
		}
	}
}