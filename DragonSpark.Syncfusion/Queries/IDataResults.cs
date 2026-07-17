using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Queries;

public interface IDataResults : ITable<DataManagerRequest, DataResult>, ICommand;