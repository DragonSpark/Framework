namespace DragonSpark.Application.Model.Sequences;

public sealed class DefaultMembershipTransaction<T, V> : MembershipTransaction<T, V> where V : T
{
	public static DefaultMembershipTransaction<T, V> Default { get; } = new DefaultMembershipTransaction<T, V>();

	DefaultMembershipTransaction() { }
}