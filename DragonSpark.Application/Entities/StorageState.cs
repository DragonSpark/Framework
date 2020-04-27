using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class StorageState : IStorageState
	{
		readonly IOperationResult<int> _confirm;
		readonly ICommand<object>      _cancel;

		public StorageState(DbContext storage) : this(new Confirm(storage), new Undo(storage)) {}

		public StorageState(IOperationResult<int> confirm, ICommand<object> cancel)
		{
			_confirm = confirm;
			_cancel  = cancel;
		}

		public ValueTask<int> Get() => _confirm.Get();

		public void Execute(object parameter)
		{
			_cancel.Execute(parameter);
		}
	}
}