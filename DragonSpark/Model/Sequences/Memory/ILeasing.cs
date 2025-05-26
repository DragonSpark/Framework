using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Model.Sequences.Memory;

public interface ILeasing<TIn, T> : IStopAware<TIn, Leasing<T>>;