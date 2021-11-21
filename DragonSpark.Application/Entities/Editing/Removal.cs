using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class Removal<TIn, T> : IOperation<TIn> where T : class
{
	readonly ISelecting<TIn, T?> _select;
	readonly Remove<T>           _remove;
	readonly ICommand<TIn>       _command;

	protected Removal(ISelecting<TIn, T?> select, Remove<T> remove)
		: this(@select, remove, EmptyCommand<TIn>.Default) {}

	protected Removal(ISelecting<TIn, T?> select, Remove<T> remove, ICommand<TIn> command)
	{
		_select  = select;
		_remove  = remove;
		_command = command;
	}

	public async ValueTask Get(TIn parameter)
	{
		var entity = await _select.Await(parameter);
		if (entity is not null)
		{
			await _remove.Await(entity);
		}
		else
		{
			_command.Execute(parameter);
		}
	}
}