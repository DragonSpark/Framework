using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class MemoryAwareStateViews<T> : Selecting<ClaimsPrincipal, StateView<T>>, IStateViews<T> where T : class
	{
		[UsedImplicitly]
		public MemoryAwareStateViews(IStateViews<T> previous, IMemoryCache memory)
			: base(previous.Then()
			               .Store()
			               .In(memory)
			               .For(TimeSpan.FromMinutes(1).Slide())
			               .Using(StateViewKey.Default)) {}
	}
}