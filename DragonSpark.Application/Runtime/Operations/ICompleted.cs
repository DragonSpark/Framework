using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Runtime.Operations;

public interface ICompleted<T> : ICondition<IResulting<T?>>, IAllocated<ValueTask<T?>>;

public interface ICompleted : IAllocated, IDisposable;