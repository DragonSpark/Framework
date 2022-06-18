using DragonSpark.Model;

namespace DragonSpark.Application.Compose.Store;

delegate TOut Get<TIn, out TOut>(Pair<object, TIn> parameter);