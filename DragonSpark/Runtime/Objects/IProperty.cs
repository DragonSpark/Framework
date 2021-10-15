using DragonSpark.Model;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Objects;

public interface IProperty<in T> : ISelect<T, Pair<string, object>> {}