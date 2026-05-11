using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

public interface IEntityProcessor<T> : IStopAware<SourceInput<T>>;