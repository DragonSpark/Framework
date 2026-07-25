using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Collections.Concurrent;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class DataResults : IDataResults
{
	readonly IDictionary<string, DataResult>        _store;
	readonly ITable<DataManagerRequest, DataResult> _previous;

	public DataResults() : this([]) {}

	public DataResults(ConcurrentDictionary<string, DataResult> store) : this(store, new ResultTable(store)) {}

	public DataResults(IDictionary<string, DataResult> store, ITable<DataManagerRequest, DataResult> previous)
	{
		_store    = store;
		_previous = previous;
	}

	public ICondition<DataManagerRequest> Condition => _previous.Condition;

	public DataResult Get(DataManagerRequest parameter) => _previous.Get(parameter);

	public void Execute(Pair<DataManagerRequest, DataResult> parameter)
	{
		_previous.Execute(parameter);
	}

	public bool Remove(DataManagerRequest key) => _previous.Remove(key);

	public void Execute(None parameter)
	{
		_store.Clear();
	}
}