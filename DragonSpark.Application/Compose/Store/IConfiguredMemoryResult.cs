using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Compose.Store;

public interface IConfiguredMemoryResult<T> : IConfiguredMemoryResult<T, T> {}

public interface IConfiguredMemoryResult<TIn, out TOut> : ISelect<(TIn Parameter, object Key), TOut> {}