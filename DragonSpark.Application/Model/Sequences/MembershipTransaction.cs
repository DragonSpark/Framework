using System.Collections.Generic;

namespace DragonSpark.Application.Model.Sequences;

public sealed class MembershipTransaction<T> : MembershipTransaction<T, T>
{
	public static MembershipTransaction<T> Default { get; } = new MembershipTransaction<T>();

	MembershipTransaction() { }
}

public class MembershipTransaction<T, V> : IMembershipTransaction<T, V> where V : T
{
	public void Execute((ICollection<T> Subject, Transactions<V> Transactions) parameter)
	{
		var (subject, transactions) = parameter;

		var (add, _, remove) = transactions.AsSpans();

		for (var index = 0; index < add.Length; index++)
		{
			var element = add[index];
			subject.Add(element);
		}

		for (var index = 0; index < remove.Length; index++)
		{
			var element = remove[index];
			subject.Remove(element);
		}
	}
}