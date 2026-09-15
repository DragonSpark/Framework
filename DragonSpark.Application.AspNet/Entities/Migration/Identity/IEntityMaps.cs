using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IEntityMaps<TFrom, TTo>
	: ISelect<IEntities, IStopAware<IReadOnlyCollection<TFrom>, IPopAware<object, Migrators.Instances.Entry<TTo>>>>
	where TTo : class;