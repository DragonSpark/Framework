using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IDestination<T> : IStopAware<PageInput<T>>;