﻿using DragonSpark.Application;
using DragonSpark.Application.Compose.Store.Operations;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.SyncfusionRendering.Components;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class MemoryAwareDataRequest : Selecting<DataManagerRequest, object>, IDataRequest
{
	public MemoryAwareDataRequest(IDataRequest previous, IMemoryCache memory, string key)
		: this(previous, new(memory, DefaultRequestMemoryTimeSpan.Default, new RequestKey(key).Get)) {}

	public MemoryAwareDataRequest(IDataRequest previous, MemoryStoreProfile<DataManagerRequest> profile)
		: base(previous.Then().Store().Using(profile)) {}
}