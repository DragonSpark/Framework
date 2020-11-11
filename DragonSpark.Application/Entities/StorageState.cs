using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class StorageState : IStorageState
	{
		readonly IResulting<int> _save;
		readonly IUndo           _undo;

		public StorageState(ISave save, IUndo undo)
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