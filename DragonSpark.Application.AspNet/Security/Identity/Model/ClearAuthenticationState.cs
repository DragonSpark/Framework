using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class ClearAuthenticationState : RemoveFromMemory<uint>, IClearAuthenticationState
{
	public ClearAuthenticationState(IMemoryCache memory) : base(memory, StateViewKey.Default) {}
}