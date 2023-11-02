using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Server.Requests;

public interface IInput<TIn, TOut> : ISelecting<Input<TIn>, TOut>;

public interface IInput<T> : IOperation<Input<T>>;