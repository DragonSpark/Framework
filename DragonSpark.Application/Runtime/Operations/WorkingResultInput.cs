using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Runtime.Operations;

public readonly record struct WorkingResultInput<T>(IResulting<T?> Previous, IAllocated Complete);