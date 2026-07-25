using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Model.Sequences;

public interface IMembershipTransaction<T, V>
	: ICommand<(ICollection<T> Subject, Transactions<V> Transactions)> where V : T;