using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IEdit<T> : IEdit<T, T>;

public interface IEdit<TIn, T> : IStopAware<TIn, Edit<T>>;