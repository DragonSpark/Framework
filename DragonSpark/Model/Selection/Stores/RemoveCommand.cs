using DragonSpark.Model.Commands;
using JetBrains.Annotations;

namespace DragonSpark.Model.Selection.Stores;

[UsedImplicitly]
class RemoveCommand<TIn, TOut> : ICommand<TIn>
{
	readonly ITable<TIn, TOut> _table;

	public RemoveCommand(ITable<TIn, TOut> table) => _table = table;

	public void Execute(TIn parameter)
	{
		_table.Remove(parameter);
	}
}