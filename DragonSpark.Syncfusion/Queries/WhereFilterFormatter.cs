using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Text;
using System;
using System.Linq;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class WhereFilterFormatter : IFormatter<Syncfusion.Blazor.Data.WhereFilter>
{
	public static WhereFilterFormatter Default { get; } = new();

	WhereFilterFormatter() : this(PredicateFormatter.Default.Get) {}

	readonly Func<Syncfusion.Blazor.Data.WhereFilter, string> _predicate;

	public WhereFilterFormatter(Func<Syncfusion.Blazor.Data.WhereFilter, string> predicate) => _predicate = predicate;

	public string Get(Syncfusion.Blazor.Data.WhereFilter parameter)
		=> $"{_predicate(parameter)}+{string.Join(' ', parameter.predicates.Account()?.Select(_predicate) ?? Empty.Enumerable<string>())}";
}