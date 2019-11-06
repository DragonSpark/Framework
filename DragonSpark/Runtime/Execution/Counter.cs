using System.Threading;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Execution
{
	public sealed class Counter : ICounter
	{
		int _count;

		public int Get() => _count;

		public void Execute(None parameter)
		{
			Interlocked.Increment(ref _count);
		}
	}

	sealed class Counter<T> : Select<T, int>
	{
		public Counter() : base(Start.A.Selection<T>()
		                             .AndOf<Counter>()
		                             .By.Instantiation.ToTable()
		                             .Select(x => x.Count())) {}
	}
}