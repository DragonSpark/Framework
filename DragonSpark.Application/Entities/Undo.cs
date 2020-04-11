using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	sealed class Undo : ICommand<object>
	{
		readonly DbContext _storage;

		public Undo(DbContext storage) => _storage = storage;

		public void Execute(object parameter)
		{
			_storage.Undo(parameter);
		}
	}
}