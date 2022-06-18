using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Compose.Store;

public interface IConfiguredMemoryResult<T> : IConfiguredMemoryResult<T, T>, IAssign<object, T?> {}

public interface IConfiguredMemoryResult<TIn, out TOut> : ISelect<Pair<object, TIn>, TOut> {}