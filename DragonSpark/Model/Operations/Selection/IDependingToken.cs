using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Model.Operations.Selection;

public interface IDependingToken<T> : ISelecting<Token<T>, bool>;

public interface IDependingToken : IDependingToken<None>;