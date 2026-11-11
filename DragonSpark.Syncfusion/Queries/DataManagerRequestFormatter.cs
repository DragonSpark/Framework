using DragonSpark.Compose;
using DragonSpark.Text;
using Syncfusion.Blazor;
using Empty = DragonSpark.Model.Empty;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class DataManagerRequestFormatter : IFormatter<DataManagerRequest>
{
	public static DataManagerRequestFormatter Default { get; } = new();

	DataManagerRequestFormatter() : this(WhereFilterFormatter.Default.Get, Empty.Enumerable<string>()) {}

	readonly Func<Syncfusion.Blazor.Data.WhereFilter, string> _where;
	readonly IEnumerable<string>                              _empty;

	public DataManagerRequestFormatter(Func<Syncfusion.Blazor.Data.WhereFilter, string> where,
	                                   IEnumerable<string> empty)
	{
		_where = where;
		_empty = empty;
	}

	public string Get(DataManagerRequest parameter)
	{
		var sorted = string.Join(',', parameter.Sorted?.Select(x => $"{x.Name} {x.Direction}") ?? _empty);
		var select = string.Join(',', parameter.Select ?? _empty);
		var where  = string.Join(',', parameter.Where?.Select(_where) ?? _empty);
		var search =
			string.Join(',',
			            parameter.Search
			                     ?.Select(x => $"{string.Join(',', x.Fields.Verify())} {x.Key} {x.Operator} {x.IgnoreCase}")
			            ?? _empty);

		var result = $"{select}_{where}_{search}_{sorted}_{parameter.Take}_{parameter.Skip}";
		return result;
	}
}