using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model;

sealed class ClearAuthenticationState : RemoveFromMemory<ClaimsPrincipal>, IClearAuthenticationState
{
	public ClearAuthenticationState(IMemoryCache memory) : base(memory, StateViewKey.Default) {}
}