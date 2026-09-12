using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMap<TFrom, TTo> : IEntityMap<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IResult<Workspace>      _enhanced;
	readonly IBuildStore<TFrom, TTo> _build;

	public EntityMap(IEntities entities) : this(entities.Enhanced, BuildStore<TFrom, TTo>.Default) {}

	public EntityMap(IResult<Workspace> enhanced, IBuildStore<TFrom, TTo> build)
	{
		_enhanced = enhanced;
		_build = build;
	}

	public async ValueTask<IPopAware<object, Migrators.Instances.Entry<TTo>>> Get(
		Stop<IReadOnlyCollection<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		await using var workspace = _enhanced.Get();

		var store = await _build.Off(new(new(workspace, subject), stop));
		
		return new StandardTable<object, Migrators.Instances.Entry<TTo>>(store);
	}
}