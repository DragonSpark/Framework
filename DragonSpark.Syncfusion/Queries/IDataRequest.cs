using DragonSpark.Model.Operations.Selection.Stop;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Queries;

public interface IDataRequest : IStopAware<DataManagerRequest, DataResult>;