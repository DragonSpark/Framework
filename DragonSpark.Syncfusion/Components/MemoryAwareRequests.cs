using DragonSpark.Model.Selection.Stores;
using DragonSpark.SyncfusionRendering.Queries;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class MemoryAwareRequests : IRequests
{
	readonly ITable<DataManagerRequest, DataResult> _store;

	public MemoryAwareRequests(ITable<DataManagerRequest, DataResult> store) => _store = store;

	public IDataRequest Get(IDataRequest parameter) => new StoreAwareDataRequest(_store, parameter);
}