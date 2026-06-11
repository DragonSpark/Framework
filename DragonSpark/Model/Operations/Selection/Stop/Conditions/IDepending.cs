namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IDepending<T> : Selection.Conditions.IDepending<Stop<T>>, IStopAware<T, bool>;

public interface IDepending : IDepending<None>;
