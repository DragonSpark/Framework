using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

public interface IInstance<TFrom, TTo> : IStopAware<MappingInput<TFrom>, TTo>;