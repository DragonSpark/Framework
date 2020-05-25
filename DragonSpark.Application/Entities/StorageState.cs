using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class StorageState : IStorageState
	{
		readonly IResulting<int>  _confirm;
		readonly ICommand<object> _cancel;

		[UsedImplicitly]
		public StorageState(DbContext storage) : this(new Confirm(storage), new Undo(storage)) {}

		public StorageState(IResulting<int> confirm, ICommand<object> cancel)
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