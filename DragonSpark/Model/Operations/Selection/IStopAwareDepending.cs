using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Model.Operations.Selection;

public interface IStopAwareDepending<T> : ISelecting<Token<T>, bool>;

public interface IStopAwareDepending : IStopAwareDepending<None>;