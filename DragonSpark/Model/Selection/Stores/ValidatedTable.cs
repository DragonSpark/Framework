using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Selection.Stores
{
	public class ValidatedTable<TIn, TOut> : ITable<TIn, TOut>
	{
		readonly ITable<TIn, TOut> _table;

		public ValidatedTable(ITable<TIn, TOut> table) : this(table.Condition, table) {}

		public ValidatedTable(ICondition<TIn> condition, ITable<TIn, TOut> table)
		{
			Condition = condition;
			_table    = table;
		}

		public void Execute(Pair<TIn, TOut> parameter)
		{
			if (Condition.Get(parameter.Key))
			{
				_table.Execute(parameter);
			}
		}

		public bool Remove(TIn key) => Condition.Get(key) && _table.Remove(key);

		public TOut Get(TIn parameter) => _table.Get(parameter);

		public ICondition<TIn> Condition { get; }
	}
}