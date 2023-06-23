using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class MemoryAwareAccessToken : Resulting<AccessToken>, IAccessToken
{
	public MemoryAwareAccessToken(IAccessToken previous, IMemoryCache memory)
		: this(previous, memory, TimeSpan.FromMinutes(59)) {}

	public MemoryAwareAccessToken(IAccessToken previous, IMemoryCache memory, TimeSpan time)
		: base(previous.Then().Accept().Then().Store().In(memory).For(time).Using<MemoryAwareAccessToken>().Bind()) {}
}