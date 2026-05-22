using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public interface ICompleted<T> : ICondition<IResulting<T?>>, IAllocated<ValueTask<T?>>;

public interface ICompleted : IAllocated, IDisposable;