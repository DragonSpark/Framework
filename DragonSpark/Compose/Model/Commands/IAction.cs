using DragonSpark.Model;
using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Commands;

public interface IAction<in T> : ISelect<T, None> {}