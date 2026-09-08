using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IEntityMap<TFrom, TTo>
	: IStopAware<IReadOnlyCollection<TFrom>, IPopAware<object, Migrators.Instances.Entry<TTo>>>
	where TTo : class;