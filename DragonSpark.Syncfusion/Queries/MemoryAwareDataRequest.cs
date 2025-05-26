using DragonSpark.Application;
using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.SyncfusionRendering.Components;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor;
using System;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class MemoryAwareDataRequest : Selecting<Stop<DataManagerRequest>, object>, IDataRequest
{
	public MemoryAwareDataRequest(IDataRequest previous, IMemoryCache memory, Func<string> key)
		: this(previous,
		       new(memory, DefaultRequestMemoryTimeSpan.Default,
		           new RequestKey(key).Then().Accept<Stop<DataManagerRequest>>(x => x.Subject).Get)) {}

	public MemoryAwareDataRequest(IDataRequest previous, StoreProfile<Stop<DataManagerRequest>> profile)
		: base(previous.Then().Store().Using(profile)) {}
}