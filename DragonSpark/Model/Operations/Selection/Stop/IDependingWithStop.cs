using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IDependingWithStop<T> : ISelecting<Stop<T>, bool>;

public class DependingWithStop<T> : StopAware<T, bool>, IDependingWithStop<T>
{
	public DependingWithStop(ISelect<Stop<T>, ValueTask<bool>> select) : base(select) {}

	public DependingWithStop(Func<Stop<T>, ValueTask<bool>> select) : base(select) {}
}

public interface IDependingWithStop : IDependingWithStop<None>;
