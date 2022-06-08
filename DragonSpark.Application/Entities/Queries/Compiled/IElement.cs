using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Application.Entities.Queries.Compiled;

public interface IElement<TIn, T> : IAllocating<In<TIn>, T> {}