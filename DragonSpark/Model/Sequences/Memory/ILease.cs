using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Memory;

public interface ILease<in TIn, T> : ISelect<TIn, Leasing<T>> {}