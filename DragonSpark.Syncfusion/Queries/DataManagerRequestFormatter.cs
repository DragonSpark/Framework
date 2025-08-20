using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Text;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class DataManagerRequestFormatter : IFormatter<DataManagerRequest>
{
	public static DataManagerRequestFormatter Default { get; } = new();

	DataManagerRequestFormatter() : this(WhereFilterFormatter.Default.Get, Empty.Enumerable<string>()) {}

	readonly Func<WhereFilter, string> _where;
	readonly IEnumerable<string>       _empty;

	public DataManagerRequestFormatter(Func<WhereFilter, string> where, IEnumerable<string> empty)
	{
		_where = where;
		_empty = empty;
	}

	public string Get(DataManagerRequest parameter)
	{
		var sorted = string.Join(',', parameter.Sorted.Account()?.Select(x => $"{x.Name} {x.Direction}") ?? _empty);
		var select = string.Join(',', parameter.Select.Account() ?? _empty);
		var where  = string.Join(',', parameter.Where.Account()?.Select(_where) ?? _empty);
		var search =
			string.Join(',',
			            parameter.Search.Account()
			                     ?
			                     .Select(x => $"{string.Join(',', x.Fields)} {x.Key} {x.Operator} {x.IgnoreCase}")
			            ?? _empty);

		var result = $"{select}_{where}_{search}_{sorted}_{parameter.Take}_{parameter.Skip}";
		return result;
	}
}