namespace DragonSpark.Model.Operations.Selection;

public interface IStopAwareDepending<T> : ISelecting<Stop<T>, bool>;

public interface IStopAwareDepending : IStopAwareDepending<None>;