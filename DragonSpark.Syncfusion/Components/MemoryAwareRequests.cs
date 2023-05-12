using DragonSpark.Application.Compose.Store.Operations;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor;
using System;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class MemoryAwareRequests : IRequests
{
	readonly MemoryStoreProfile<DataManagerRequest> _profile;

	public MemoryAwareRequests(IMemoryCache memory, string key) : this(memory, TimeSpan.FromSeconds(1), key) {}

	public MemoryAwareRequests(IMemoryCache memory, TimeSpan @for, string key) : this(new(memory, @for, key)) {}

	public MemoryAwareRequests(MemoryStoreProfile<DataManagerRequest> profile) => _profile = profile;

	public IDataRequest Get(IDataRequest parameter) => new MemoryAwareDataRequest(parameter, _profile);
}