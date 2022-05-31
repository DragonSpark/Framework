using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Runtime;

public readonly record struct Throttle<T>(T Parameter, Operate<T> callback);

