using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Operation<T> : Select<T, ValueTask>, IOperation<T>
{
	public Operation(ISelect<T, ValueTask> select) : this(select.Get) {}

	public Operation(Func<T, ValueTask> select) : base(select) {}
}

public class Operation : IOperation
{
	readonly Func<ValueTask> _select;

	public Operation(Func<ValueTask> select) => _select = select;

	public ValueTask Get() => _select();
}