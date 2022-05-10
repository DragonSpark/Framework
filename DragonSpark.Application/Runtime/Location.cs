using System;

namespace DragonSpark.Application.Runtime;

public readonly record struct Location<T>(Memory<T> Inputs, T Stored);