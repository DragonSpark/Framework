using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMap<TFrom, TTo> : IEntityMap<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IOriginAware              _workspaces;
	readonly IBuildStore<TFrom, TTo>   _build;

	public EntityMap(IOriginAware workspaces) : this(workspaces, BuildStore<TFrom, TTo>.Default) {}

	public EntityMap(IOriginAware workspaces, IBuildStore<TFrom, TTo> build)
	{
		_workspaces = workspaces;
		_build = build;
	}

	public async ValueTask<IPopAware<object, Migrators.Instances.Entry<TTo>>> Get(
		Stop<IReadOnlyCollection<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		await using var workspace = _workspaces.Origin();

		var store = await _build.Off(new(new(workspace, subject), stop));
		
		return new StandardTable<object, Migrators.Instances.Entry<TTo>>(store);
	}
}