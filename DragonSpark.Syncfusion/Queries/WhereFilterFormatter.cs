using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Text;
using Syncfusion.Blazor.Data;
using System;
using System.Linq;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class WhereFilterFormatter : IFormatter<WhereFilter>
{
	public static WhereFilterFormatter Default { get; } = new();

	WhereFilterFormatter() : this(PredicateFormatter.Default.Get) {}

	readonly Func<WhereFilter, string> _predicate;

	public WhereFilterFormatter(Func<WhereFilter, string> predicate) => _predicate = predicate;

	public string Get(WhereFilter parameter)
		=> $"{_predicate(parameter)}+{string.Join(' ', parameter.predicates.Account()?.Select(_predicate) ?? Empty.Enumerable<string>())}";
}