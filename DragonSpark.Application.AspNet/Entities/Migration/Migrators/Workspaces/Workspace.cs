using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public readonly record struct Workspace(DbContext Source, DbContext Destination) : IDisposable, IAsyncDisposable
{
	public void Dispose()
	{
		Source.Dispose();
		Destination.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		await Source.DisposeAsync().Off();
		await Destination.DisposeAsync().Off();
	}
}