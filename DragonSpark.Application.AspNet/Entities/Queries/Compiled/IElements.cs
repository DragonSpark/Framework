using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled;

public interface IElements<TIn, out T> : ISelect<In<TIn>, IAsyncEnumerable<T>>;