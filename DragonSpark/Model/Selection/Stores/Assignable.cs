using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;

namespace DragonSpark.Model.Selection.Stores;

[UsedImplicitly]
public class Assignable<TIn, TOut> : IAssignable<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _select;
	readonly IAssign<TIn, TOut> _assign;

	public Assignable(IAssignable<TIn, TOut> previous) : this(previous.Condition, previous, previous) {}

	public Assignable(ICondition<TIn> condition, ISelect<TIn, TOut> select, IAssign<TIn, TOut> assign)
	{
		Condition = condition;
		_select   = @select;
		_assign   = assign;
	}

	public ICondition<TIn> Condition { get; }

	public TOut Get(TIn parameter) => _select.Get(parameter);

	public void Execute(Pair<TIn, TOut> parameter)
	{
		_assign.Execute(parameter);
	}
}