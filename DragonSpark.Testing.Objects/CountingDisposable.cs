using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Testing.Objects
{
	public sealed class CountingDisposable : Disposable, IResult<int>
	{
		readonly ICounter _counter;

		public CountingDisposable() : this(new Counter()) {}

		public CountingDisposable(ICounter counter) : base(counter.Execute) => _counter = counter;

		public int Get() => _counter.Get();
	}
}