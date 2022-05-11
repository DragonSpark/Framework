using DragonSpark.Model.Commands;
using System.Collections.Generic;

namespace DragonSpark.Application.Model.Sequences;

public interface IMembershipTransaction<T, V> : ICommand<(ICollection<T> Subject, Transactions<V> Transactions)>
	where V : T
{ }