using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IDependingWithStop<T> : IDepending<Stop<T>>;

public class DependingWithStop<T> : Depending<Stop<T>>, IDependingWithStop<T>
{
	public DependingWithStop(ISelect<Stop<T>, ValueTask<bool>> select) : base(select) {}

	public DependingWithStop(Func<Stop<T>, ValueTask<bool>> select) : base(select) {}
}

public interface IDependingWithStop : IDependingWithStop<None>;
