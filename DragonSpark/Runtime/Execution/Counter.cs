using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System.Threading;

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

	[UsedImplicitly]
	public sealed class Counter<T> : Select<T, int> where T : notnull
	{
		public Counter() : base(Start.A.Selection<T>()
		                             .AndOf<Counter>()
		                             .By.Instantiation.Get()
		                             .ToTable()
		                             .Select(x => x.Count())) {}
	}
}