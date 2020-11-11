using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Entities
{
	sealed class Undo : IUndo
	{
		readonly DbContext _storage;

		public Undo(DbContext storage) => _storage = storage;

		public void Execute(object parameter)
		{
			Revert(_storage.Entry(parameter));
		}

		static void Revert(EntityEntry entry)
		{
			switch (entry.State)
			{
				case EntityState.Modified:
					entry.State = EntityState.Unchanged;
					break;
				case EntityState.Added:
					entry.State = EntityState.Detached;
					break;
				case EntityState.Deleted:
					entry.Reload();
					break;
			}
		}

		public void Execute(None parameter)
		{
			foreach (var entry in _storage.ChangeTracker.Entries()
			                              .AsValueEnumerable()
			                              .Where(e => e.State != EntityState.Unchanged))
			{
				Revert(entry!);
			}
		}
	}
}