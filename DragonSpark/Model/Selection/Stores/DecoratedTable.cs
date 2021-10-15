using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Selection.Stores;

public class DecoratedTable<TIn, TOut> : ITable<TIn, TOut>
{
	readonly ITable<TIn, TOut> _source;

	public DecoratedTable(ITable<TIn, TOut> source) : this(source.Condition, source) {}

	public DecoratedTable(ICondition<TIn> condition, ITable<TIn, TOut> source)
	{
		Condition = condition;
		_source   = source;
	}

	public ICondition<TIn> Condition { get; }

	public void Execute(Pair<TIn, TOut> parameter) => _source.Execute(parameter);

	public bool Remove(TIn key) => _source.Remove(key);

	public TOut Get(TIn parameter) => _source.Get(parameter);
}