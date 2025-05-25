using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Server.Requests;

public interface IInput<TIn, TOut> : IStopAware<Input<TIn>, TOut>;

public interface IInput<T> : IStopAware<Input<T>>;