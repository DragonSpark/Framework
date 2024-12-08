using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Text;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.State;

sealed class MemoryAwareStateUser<T> : Selecting<ClaimsPrincipal, T?>, IStateUser<T> where T : IdentityUser
{
	public MemoryAwareStateUser(IStateUser<T> previous, IMemoryCache memory)
		: this(previous, memory, StateViewMemoryKey.Default) {}

	public MemoryAwareStateUser(IStateUser<T> previous, IMemoryCache memory, IFormatter<ClaimsPrincipal> key)
		: base(previous.Then().Store().In(memory).For(TimeSpan.FromMinutes(10)).Using(key)) {}
}