namespace DragonSpark.Model.Operations.Selection.Conditions;

public interface IDepending : IDepending<None> {}

public interface IDepending<in T> : ISelecting<T, bool> {}