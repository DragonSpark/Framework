using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System.Threading;

namespace DragonSpark.Runtime.Execution;

public sealed class SafeCounter : ICounter
{
	int _count;

	public int Get() => _count;

	public void Execute(None parameter)
	{
		Interlocked.Increment(ref _count);
	}
}
public sealed class SafeCounter<T> : Select<T, int> where T : notnull
{
	public SafeCounter() : base(Start.A.Selection<T>()
	                             .AndOf<SafeCounter>()
	                             .By.Instantiation.Get()
	                             .ToTable()
	                             .Select(x => x.Count())) {}
}
