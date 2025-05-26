using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

sealed class RemovalDispatch<TIn, T> : IStopAware<TIn> where T : class
{
	readonly IStopAware<TIn, T?> _select;
	readonly Remove<T>           _remove;
	readonly ICommand<TIn>       _command;

	public RemovalDispatch(IStopAware<TIn, T?> select, Remove<T> remove, ICommand<TIn> command)
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
			await _remove.Off(new(entity, parameter));
		}
		else
		{
			_command.Execute(parameter);
		}
	}
}