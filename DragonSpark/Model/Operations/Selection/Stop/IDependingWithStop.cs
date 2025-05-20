namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IDependingWithStop<T> : ISelecting<Stop<T>, bool>;

public interface IDependingWithStop : IDependingWithStop<None>;
