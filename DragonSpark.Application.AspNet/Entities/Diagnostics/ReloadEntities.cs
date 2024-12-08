using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class ReloadEntities : IOperation<(DbUpdateConcurrencyException, TimeSpan)>
{
	public static ReloadEntities Default { get; } = new();

	ReloadEntities() {}

	public async ValueTask Get((DbUpdateConcurrencyException, TimeSpan) parameter)
	{
		var (exception, _) = parameter;
		foreach (var entry in exception.Entries)
		{
			await entry.ReloadAsync().Await();
		}
	}
}