using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor;
using System;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class MemoryAwareRequests : IRequests
{
	readonly StoreProfile<DataManagerRequest> _profile;

	public MemoryAwareRequests(IMemoryCache memory, TimeSpan @for, string key)
		: this(new(memory, @for, new RequestKey(key).Get)) {}

	public MemoryAwareRequests(StoreProfile<DataManagerRequest> profile) => _profile = profile;

	public IDataRequest Get(IDataRequest parameter) => new MemoryAwareDataRequest(parameter, _profile);
}