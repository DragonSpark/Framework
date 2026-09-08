using DragonSpark.Model.Operations.Allocated.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public interface IWriter<TFrom> : IAllocated<WriterInput<TFrom>>;