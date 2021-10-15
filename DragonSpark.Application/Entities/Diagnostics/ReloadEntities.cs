using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Diagnostics;

sealed class ReloadEntities : IOperation<(DbUpdateConcurrencyException, TimeSpan)>
{
	public static ReloadEntities Default { get; } = new ReloadEntities();

	ReloadEntities() {}

	public async ValueTask Get((DbUpdateConcurrencyException, TimeSpan) parameter)
	{
		var (exception, _) = parameter;
		foreach (var entry in exception.Entries.AsValueEnumerable())
		{
			await entry.ReloadAsync().ConfigureAwait(false);
		}
	}
}