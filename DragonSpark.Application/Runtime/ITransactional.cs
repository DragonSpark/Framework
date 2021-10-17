using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Runtime;

public interface ITransactional<T> : ISelect<TransactionInput<T>, Transactions<T>> {}

public readonly record struct TransactionInput<T>(Memory<T> Stored, Memory<T> Source);