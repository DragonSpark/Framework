using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Compiled;

public interface IElements<TIn, out T> : ISelect<In<TIn>, IAsyncEnumerable<T>> {}