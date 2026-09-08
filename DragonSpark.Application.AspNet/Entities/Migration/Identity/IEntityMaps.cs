using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IEntityMaps<TFrom, TTo>
	: ISelect<IResult<Workspace>,
		IStopAware<IReadOnlyCollection<TFrom>, IPopAware<object, Migrators.Instances.Entry<TTo>>>>
	where TTo : class;