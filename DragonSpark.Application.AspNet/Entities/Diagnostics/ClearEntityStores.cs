using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class ClearEntityStores : ICommand<(DbUpdateConcurrencyException, TimeSpan)>
{
	public static ClearEntityStores Default { get; } = new();

	ClearEntityStores() {}

	public void Execute((DbUpdateConcurrencyException, TimeSpan) parameter)
	{
		var (exception, _) = parameter;
		using var lease = exception.Entries.Select(x => x.Context)
		                           .Distinct()
		                           .AsValueEnumerable()
		                           .ToArray(ArrayPool<DbContext>.Shared);
		foreach (var context in lease)
		{
			context.ChangeTracker.Clear();
		}
	}
}