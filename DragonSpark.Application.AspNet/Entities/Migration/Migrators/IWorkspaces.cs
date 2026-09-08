using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IWorkspaces : IResult<Workspace>
{
	DatabaseFacade Database { get; }
	IModel Model { get; }
}

// TODO

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