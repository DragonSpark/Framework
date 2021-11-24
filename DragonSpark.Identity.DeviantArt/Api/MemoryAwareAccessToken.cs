using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class MemoryAwareAccessToken : Resulting<AccessToken>, IAccessToken
{
	public MemoryAwareAccessToken(IAccessToken previous, IMemoryCache memory)
		: this(previous, memory, TimeSpan.FromMinutes(59)) {}

	public MemoryAwareAccessToken(IAccessToken previous, IMemoryCache memory, TimeSpan time)
		: base(previous.Then().Accept().Store().In(memory).For(time).Using<MemoryAwareAccessToken>().Bind()) {}
}