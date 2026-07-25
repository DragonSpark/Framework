using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ReportedDataRequest : Reporting<DataManagerRequest, DataResult>, IDataRequest
{
	public ReportedDataRequest(IStopAware<DataManagerRequest, DataResult> previous, Action<Task> report)
		: base(previous, report) {}
}