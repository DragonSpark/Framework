using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Composition.Compose;

public interface IHostAccessor<T> : ISelect<IDictionary<object, object>, T?>, IAssign<IDictionary<object, object>, T> where T : class;