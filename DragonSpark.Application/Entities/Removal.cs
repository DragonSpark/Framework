using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Removal<TIn, T> : IOperation<TIn>
	{
		readonly ISelecting<TIn, T?> _select;
		readonly IRemove<T>          _remove;
		readonly ICommand<TIn>       _message;

		public Removal(ISelecting<TIn, T?> select, IRemove<T> remove, ICommand<TIn> message)
		{
			_select  = @select;
			_remove  = remove;
			_message = message;
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
				_message.Execute(parameter);
			}
		}
	}
}