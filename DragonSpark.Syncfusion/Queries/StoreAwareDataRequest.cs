using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection.Stores;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class StoreAwareDataRequest : LockAwareStoring<DataManagerRequest, DataResult>, IDataRequest
{
	public StoreAwareDataRequest(ITable<DataManagerRequest, DataResult> store, IDataRequest previous)
		: base(store, previous.Get) {}
}