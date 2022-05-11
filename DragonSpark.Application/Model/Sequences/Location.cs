using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct Location<T>(Memory<T> Inputs, T Stored);