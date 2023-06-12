using DragonSpark.Model;
using DragonSpark.Text;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System;
using System.Linq;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class DataManagerRequestFormatter : IFormatter<DataManagerRequest>
{
	public static DataManagerRequestFormatter Default { get; } = new();

	DataManagerRequestFormatter() : this(WhereFilterFormatter.Default.Get) {}

	readonly Func<WhereFilter, string> _where;

	public DataManagerRequestFormatter(Func<WhereFilter, string> where) => _where = where;

	public string Get(DataManagerRequest parameter)
	{
		var empty  = Empty.Enumerable<string>();
		var sorted = string.Join(',', parameter.Sorted?.Select(x => $"{x.Name} {x.Direction}") ?? empty);
		var select = string.Join(',', parameter.Select ?? empty);
		var where  = string.Join(',', parameter.Where?.Select(_where) ?? empty);
		var search =
			string.Join(',',
			            parameter.Search?
				            .Select(x => $"{string.Join(',', x.Fields)} {x.Key} {x.Operator} {x.IgnoreCase}")
			            ?? empty);

		var result = $"{select}_{where}_{search}_{sorted}_{parameter.Take}_{parameter.Skip}";
		return result;
	}
}