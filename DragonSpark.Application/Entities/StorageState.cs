using DragonSpark.Model;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class StorageState : IStorageState
	{
		readonly IResulting<int> _confirm;
		readonly IUndo           _undo;

		[UsedImplicitly]
		public StorageState(DbContext storage) : this(new Confirm(storage), new Undo(storage)) {}

		public StorageState(IResulting<int> confirm, IUndo undo)
		{
			_confirm = confirm;
			_undo    = undo;
		}

		public ValueTask<int> Get() => _confirm.Get();

		public void Execute(object parameter)
		{
			_undo.Execute(parameter);
		}

		public void Execute(None parameter)
		{
			_undo.Execute(parameter);
		}
	}
}