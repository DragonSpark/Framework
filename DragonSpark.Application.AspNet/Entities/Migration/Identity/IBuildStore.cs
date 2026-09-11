using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IBuildStore<TFrom, TTo> 
	: IStopAware<BuildStoreInput<TFrom>, IDictionary<object, Migrators.Instances.Entry<TTo>>> 
	where TFrom : class where TTo : class;