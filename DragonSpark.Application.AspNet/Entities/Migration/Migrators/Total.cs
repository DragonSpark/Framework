using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Total<T> : IStopAware<IWorkspaces, uint>
{
	readonly Func<DbContext, IQueryable<T>> _query;

	public Total(Func<DbContext, IQueryable<T>> query) => _query = query;

	public async ValueTask<uint> Get(Stop<IWorkspaces> parameter)
	{
		var (subject, stop) = parameter;
		await using var workspace = subject.Get();
		var (source, _) = workspace;
		var query = _query(source);
		var count = await query.CountAsync(stop).Off();
		return count.Grade();
	}
}