using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct TransactionMemory<T>(Memory<T> Add, Memory<Update<T>> Update, Memory<T> Delete);