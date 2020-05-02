using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public sealed class StorageState<T> : IStorageState where T : DbContext
	{
		readonly IOperationResult<int> _confirm;
		readonly ICommand<object>      _cancel;

		[UsedImplicitly]
		public StorageState(T storage) : this(new Confirm(storage), new Undo(storage)) {}

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