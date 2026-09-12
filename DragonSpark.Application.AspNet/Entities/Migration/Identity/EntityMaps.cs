using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMaps<TFrom, TTo>
	: ReferenceValueStore<IEntities,
		  IStopAware<IReadOnlyCollection<TFrom>, IPopAware<object, Migrators.Instances.Entry<TTo>>>>,
	  IEntityMaps<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public static EntityMaps<TFrom, TTo> Default { get; } = new();

	EntityMaps() : base(x => new EntityMap<TFrom, TTo>(x).AsReferenceStoring()) {}
}