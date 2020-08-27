using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	sealed class StoredStateViews<T> : Selecting<ClaimsPrincipal, StateView<T>>, IStateViews<T> where T : class
	{
		[UsedImplicitly]
		public StoredStateViews(IStateViews<T> previous, IMemoryCache memory)
			: base(previous.Then()
			               .Store()
			               .In(memory)
			               .For(TimeSpan.FromSeconds(10))
			               .Using(StoredStateKey<T>.Default)) {}
	}

	sealed class StoredStateKey<T> : Key<ClaimsPrincipal>
	{
		public static StoredStateKey<T> Default { get; } = new StoredStateKey<T>();

		StoredStateKey() : base(nameof(StoredStateKey<T>), x => x.UserName()) {}
	}
}