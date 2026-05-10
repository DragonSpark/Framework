using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IInstance<TFrom, TTo> : IStopAware<MappingInput<TFrom>, TTo>;