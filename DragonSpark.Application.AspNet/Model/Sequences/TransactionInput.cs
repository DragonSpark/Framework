using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct TransactionInput<T>(Memory<T> Stored, Memory<T> Source);