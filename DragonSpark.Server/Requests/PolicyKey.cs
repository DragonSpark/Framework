using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;

namespace DragonSpark.Server.Requests
{
	public sealed class PolicyKey<T> : Key<Unique>
	{
		public static PolicyKey<T> Default { get; } = new PolicyKey<T>();

		PolicyKey() : base(A.Type<T>(), query => $"{query.UserName}+{query.Identity}") {}
	}
}