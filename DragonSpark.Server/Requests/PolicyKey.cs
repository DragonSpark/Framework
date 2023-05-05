using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;

namespace DragonSpark.Server.Requests;

public sealed class PolicyKey<T> : Key<Unique>
{
	public static PolicyKey<T> Default { get; } = new ();

	PolicyKey() : base(A.Type<T>(), x => $"{x.User}+{x.Identity}") {}
}