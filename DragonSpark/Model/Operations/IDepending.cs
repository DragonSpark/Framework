namespace DragonSpark.Model.Operations;

public interface IDepending : IDepending<None> {}

public interface IDepending<in T> : ISelecting<T, bool> {}