using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor;
using System;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class MemoryAwareDataRequest : Selecting<DataManagerRequest,object>, IDataRequest
{
	public MemoryAwareDataRequest(IDataRequest previous, IMemoryCache memory, string key) 
		: base(previous.Then().Store().In(memory).For(TimeSpan.FromSeconds(1)).Using(key.Accept)) {}
}