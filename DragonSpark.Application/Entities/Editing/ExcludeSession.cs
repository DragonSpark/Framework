using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Editing
{
	public readonly struct ExcludeSession : IDisposable
	{
		readonly IEnumerable<EntityEntry> _entries;
		readonly Func<EntityEntry, bool>  _where;

		public ExcludeSession(IEnumerable<EntityEntry> entries, Func<EntityEntry, bool> where)
		{
			_entries = entries;
			_where   = where;
		}

		public void Dispose()
		{
			foreach (var entry in _entries.AsValueEnumerable().Where(_where))
			{
				entry.State = EntityState.Detached;
			}
		}
	}
}