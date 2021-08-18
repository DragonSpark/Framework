using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public readonly struct ExcludeSession : IAsyncDisposable
	{
		readonly IEnumerable<EntityEntry> _entries;
		readonly Func<EntityEntry, bool>  _where;

		public ExcludeSession(IEnumerable<EntityEntry> entries, Func<EntityEntry, bool> where)
		{
			_entries = entries;
			_where   = where;
		}

		public ValueTask DisposeAsync()
		{
			foreach (var entry in _entries.AsValueEnumerable().Where(_where))
			{
				entry.State = EntityState.Detached;
			}

			return Task.CompletedTask.ToOperation();
		}
	}
}