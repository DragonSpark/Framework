using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMaps<TFrom, TTo>
	: ReferenceValueStore<IResult<Workspace>,
		  IStopAware<IReadOnlyCollection<TFrom>, IPopAware<object, Migrators.Instances.Entry<TTo>>>>,
	  IEntityMaps<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public static EntityMaps<TFrom, TTo> Default { get; } = new();

	EntityMaps() : base(x => new EntityMap<TFrom, TTo>(x).AsReferenceStoring()) {}
}