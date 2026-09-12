using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

sealed class Enhanced<T> : IResult<Workspace> where T : class
{
	readonly IWorkspaces _previous;
	readonly DbContext   _origin;

	public Enhanced(IWorkspaces previous, DbContext origin)
	{
		_previous = previous;
		_origin   = origin;
	}

	public Workspace Get()
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