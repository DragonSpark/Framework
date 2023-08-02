using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Model.Sequences;

public interface ITransactional<T> : ISelect<TransactionInput<T>, Transactions<T>> { }