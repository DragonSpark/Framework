using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication.State;

sealed class MemoryAwareStateViews<T> : Selecting<ClaimsPrincipal, StateView<T>>, IStateViews<T> where T : IdentityUser
{
	public MemoryAwareStateViews(IStateViews<T> previous, IMemoryCache memory)
		: base(previous.Then().Store().In(memory).For(TimeSpan.FromSeconds(1).Slide()).UsingSelf()) {}
}