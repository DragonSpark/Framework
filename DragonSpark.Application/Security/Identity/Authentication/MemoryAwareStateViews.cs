using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class MemoryAwareStateViews<T> : Selecting<ClaimsPrincipal, StateView<T>>, IStateViews<T> where T : IdentityUser
{
	[UsedImplicitly]
	public MemoryAwareStateViews(IStateViews<T> previous, IMemoryCache memory)
		: base(previous.Then()
		               .Store()
		               .In(memory)
		               .For(TimeSpan.FromMinutes(10))
		               .Using(Start.A.Selection<ClaimsPrincipal>()
		                           .By.Calling(x => x.Number())
		                           .Select(x => x.HasValue ? StateViewKey.Default.Get(x.Value) : string.Empty)
		                           .Get())) {}
}