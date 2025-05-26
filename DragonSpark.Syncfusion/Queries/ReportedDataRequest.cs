using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using Syncfusion.Blazor;
using System;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ReportedDataRequest : Reporting<Stop<DataManagerRequest>, object>, IDataRequest
{
	public ReportedDataRequest(ISelecting<Stop<DataManagerRequest>, object> previous, Action<Task> report)
		: base(previous, report) {}
}