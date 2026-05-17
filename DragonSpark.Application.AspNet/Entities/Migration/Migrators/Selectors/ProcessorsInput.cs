using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

public readonly record struct ProcessorsInput<T>(Contexts<T> Contexts, IMap Map) where T : class;