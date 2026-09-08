using DragonSpark.Model.Operations.Allocated.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IElement<TFrom, TTo> : IAllocated<MappingInput<TFrom>, TTo> where TFrom : class where TTo : class;