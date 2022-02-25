using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Results;

public class SelectedVariable<TIn, TOut> : SelectedResult<TIn, TOut>, IMutable<TOut>, ICondition
{
	readonly IResult<TIn>      _previous;
	readonly ITable<TIn, TOut> _assigned;

	protected SelectedVariable(IResult<TIn> previous, ISelect<TIn, TOut> select, ITable<TIn, TOut> assigned)
		: base(previous, @select)
	{
		_previous = previous;
		_assigned = assigned;
	}

	public void Execute(TOut parameter)
	{
		_assigned.Assign(_previous.Get(), parameter);
	}

	public bool Get(None parameter) => _assigned.Remove(_previous.Get());
}