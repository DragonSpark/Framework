using System;

namespace DragonSpark.Application.Runtime;

public readonly record struct Throttle<T>(T Parameter, Action<T> callback);