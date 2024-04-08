using DragonSpark.Application;
using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.SyncfusionRendering.Components;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor;
using System;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class MemoryAwareDataRequest : Selecting<DataManagerRequest, object>, IDataRequest
{
	public MemoryAwareDataRequest(IDataRequest previous, IMemoryCache memory, Func<string> key)
		: this(previous, new(memory, DefaultRequestMemoryTimeSpan.Default, new RequestKey(key).Get)) {}

	public MemoryAwareDataRequest(IDataRequest previous, StoreProfile<DataManagerRequest> profile)
		: base(previous.Then().Store().Using(profile)) {}
}