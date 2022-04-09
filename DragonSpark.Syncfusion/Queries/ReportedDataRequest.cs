using DragonSpark.Model.Operations;
using Syncfusion.Blazor;
using System;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ReportedDataRequest : Reporting<DataManagerRequest, object>, IDataRequest
{
	public ReportedDataRequest(ISelecting<DataManagerRequest, object> previous, Action<Task> report)
		: base(previous, report) {}
}