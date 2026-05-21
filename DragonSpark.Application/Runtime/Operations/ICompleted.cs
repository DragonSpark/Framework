using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public interface ICompleted<T> : IAllocated<ValueTask<T?>>, ICondition<IResulting<T?>>;

public interface ICompleted : IAllocated, IDisposable;