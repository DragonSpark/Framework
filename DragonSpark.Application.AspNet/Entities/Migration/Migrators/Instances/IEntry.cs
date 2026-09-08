using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

public interface IEntry<TFrom, TTo> : IStopAware<MappingInput<TFrom>, Entry<TTo>> where TFrom : class where TTo : class;