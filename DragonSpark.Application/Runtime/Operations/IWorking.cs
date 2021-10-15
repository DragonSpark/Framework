using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Runtime.Operations;

public interface IWorking<in TIn, TOut> : ISelect<TIn, Worker<TOut>> {}

public interface IWorking<in T> : ISelect<T, Worker> {}