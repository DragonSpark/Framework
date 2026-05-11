using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IMap : IStopAware<MapInput>;

public interface IMap<TFrom, TTo> : IStopAware<MapInput<TFrom, TTo>>;