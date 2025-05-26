using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor;
using System;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class MemoryAwareRequests : IRequests
{
	readonly StoreProfile<Stop<DataManagerRequest>> _profile;

	public MemoryAwareRequests(IMemoryCache memory, TimeSpan @for, string key)
		: this(new(memory, @for, new RequestKey(key).Then().Accept<Stop<DataManagerRequest>>(x => x.Subject).Get)) {}

	public MemoryAwareRequests(StoreProfile<Stop<DataManagerRequest>> profile) => _profile = profile;

	public IDataRequest Get(IDataRequest parameter) => new MemoryAwareDataRequest(parameter, _profile);
}