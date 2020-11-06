using DragonSpark.Model;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class StorageState : IStorageState
	{
		readonly IResulting<int> _save;
		readonly IUndo           _undo;

		[UsedImplicitly]
		public StorageState(DbContext storage) : this(new Save(storage), new Undo(storage)) {}

		public StorageState(IResulting<int> save, IUndo undo)
		{
			_save = save;
			_undo = undo;
		}

		public ValueTask<int> Get() => _save.Get();

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