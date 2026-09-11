using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class OriginAwareWorkspaces<T> : IOriginAware where T : class
{
	readonly IWorkspaces _previous;
	readonly DbContext   _origin;

	public OriginAwareWorkspaces(IWorkspaces previous, DbContext origin)
	{
		_previous = previous;
		_origin   = origin;
	}

	public Workspace Get() => _previous.Get();

	public DatabaseFacade Database => _previous.Database;

	public IModel Model => _previous.Model;

	public Workspace Origin()
	{
		var result = _previous.Get();
		var (source, _) = result;
		
		foreach (var entry in _origin.ChangeTracker.Entries<T>())
		{
			source.Applied(entry);
		}

		return result;
	}
}