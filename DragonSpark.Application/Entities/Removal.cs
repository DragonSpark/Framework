using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Removal<TIn, TContext, T> : IOperation<TIn> where TContext : DbContext where T : class
	{
		readonly ISelecting<TIn, T?>            _select;
		readonly Remove<TContext, T> _remove;
		readonly ICommand<TIn>                  _command;

		protected Removal(ISelecting<TIn, T?> select, Remove<TContext, T> remove, ICommand<TIn> command)
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
}