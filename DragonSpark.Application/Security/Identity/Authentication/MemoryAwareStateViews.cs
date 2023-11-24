using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class MemoryAwareStateViews<T> : Selecting<ClaimsPrincipal, StateView<T>>, IStateViews<T> where T : IdentityUser
{
	public MemoryAwareStateViews(IStateViews<T> previous, IMemoryCache memory)
		: this(previous, memory, StateViewMemoryKey.Default) {}

	[UsedImplicitly]
	public MemoryAwareStateViews(IStateViews<T> previous, IMemoryCache memory, IFormatter<ClaimsPrincipal> key)
		: base(previous.Then().Store().In(memory).For(TimeSpan.FromMinutes(10)).Using(key)) {}
}