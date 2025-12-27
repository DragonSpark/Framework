using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ReportedDataRequest : Reporting<Stop<DataManagerRequest>, DataResult>, IDataRequest
{
	public ReportedDataRequest(ISelecting<Stop<DataManagerRequest>, DataResult> previous, Action<Task> report)
		: base(previous, report) {}
}