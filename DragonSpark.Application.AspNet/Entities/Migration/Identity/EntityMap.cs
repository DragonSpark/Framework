using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMap<TFrom, TTo> : IEntityMap<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly Workspace               _workspace;
	readonly IBuildStore<TFrom, TTo> _build;

	public EntityMap(Workspace workspace) : this(workspace, BuildStore<TFrom, TTo>.Default) {}

	public EntityMap(Workspace workspace, IBuildStore<TFrom, TTo> build)
	{
		_workspace = workspace;
		_build     = build;
	}

	public async ValueTask<IPopAware<object, Migrators.Instances.Entry<TTo>>> Get(
		Stop<IReadOnlyCollection<TFrom>> parameter)
	{
		var (subject, stop) = parameter;

		var store = await _build.Off(new(new(_workspace, subject), stop));

		return new StandardTable<object, Migrators.Instances.Entry<TTo>>(store);
	}
}