using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Removal<TIn, T> : StopAdaptor<TIn> where T : class
{
	protected Removal(IStopAware<TIn, T?> select, Remove<T> remove)
		: this(@select, remove, EmptyCommand<TIn>.Default) {}

	protected Removal(IStopAware<TIn, T?> select, Remove<T> remove, ICommand<TIn> command)
		: base(new RemovalWithStop<TIn, T>(select, remove, command)) {}
}

// TODO

sealed class RemovalWithStop<TIn, T> : IStopAware<TIn> where T : class
{
	readonly IStopAware<TIn, T?> _select;
	readonly Remove<T>           _remove;
	readonly ICommand<TIn>       _command;

	public RemovalWithStop(IStopAware<TIn, T?> select, Remove<T> remove, ICommand<TIn> command)
	{
		_select  = select;
		_remove  = remove;
		_command = command;
	}

	public async ValueTask Get(Stop<TIn> parameter)
	{
		var entity = await _select.Off(parameter);
		if (entity is not null)
		{
			await _remove.Off(entity);
		}
		else
		{
			_command.Execute(parameter);
		}
	}
}