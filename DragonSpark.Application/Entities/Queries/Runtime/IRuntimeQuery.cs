using DragonSpark.Model;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public interface IRuntimeQuery<in TIn, TOut> : ISelect<TIn, IQueries<TOut>> {}

public interface IRuntimeQuery<T> : IRuntimeQuery<None, T> {}