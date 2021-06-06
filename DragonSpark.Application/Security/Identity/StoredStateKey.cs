using DragonSpark.Application.Compose.Store;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	sealed class StoredStateKey<T> : Key<ClaimsPrincipal>
	{
		public static StoredStateKey<T> Default { get; } = new StoredStateKey<T>();

		StoredStateKey() : base(nameof(StoredStateKey<T>), x => x.UserName()) {}
	}
}