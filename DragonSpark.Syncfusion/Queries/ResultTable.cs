using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Collections.Concurrent;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ResultTable : ITable<DataManagerRequest, DataResult>
{
	readonly ITable<string, DataResult>     _previous;
	readonly IFormatter<DataManagerRequest> _formatter;

	public ResultTable(ConcurrentDictionary<string, DataResult> store)
		: this(new ConcurrentTable<string, DataResult>(store)) {}

	public ResultTable(ITable<string, DataResult> previous) : this(previous, DataManagerRequestFormatter.Default) {}

	public ResultTable(ITable<string, DataResult> previous, IFormatter<DataManagerRequest> formatter)
		: this(previous, formatter, new Contains(previous.Condition, formatter)) {}

	public ResultTable(ITable<string, DataResult> previous, IFormatter<DataManagerRequest> formatter,
	                   ICondition<DataManagerRequest> condition)
	{
		_previous  = previous;
		_formatter = formatter;
		Condition  = condition;
	}

	public ICondition<DataManagerRequest> Condition { get; }

	public DataResult Get(DataManagerRequest parameter) => _previous.Get(_formatter.Get(parameter));

	public void Execute(Pair<DataManagerRequest, DataResult> parameter)
	{
		var (key, value) = parameter;
		_previous.Execute((_formatter.Get(key), value));
	}

	public bool Remove(DataManagerRequest key) => _previous.Remove(_formatter.Get(key));

	sealed class Contains : Condition<DataManagerRequest>
	{
		public Contains(ICondition<string> previous, IFormatter<DataManagerRequest> formatter)
			: base(formatter.Then().Select(previous)) {}
	}
}