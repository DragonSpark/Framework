using DragonSpark.Model.Selection.Conditions;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class IsEntirelyExact : ICondition<IsExactInput>
{
	public static IsEntirelyExact Default { get; } = new();

	IsEntirelyExact() : this(IsExact.Default, HashSet<string>.CreateSetComparer()) {}

	readonly ICondition<IsExactInput>           _previous;
	readonly IEqualityComparer<HashSet<string>> _comparer;

	public IsEntirelyExact(ICondition<IsExactInput> previous, IEqualityComparer<HashSet<string>> comparer)
	{
		_previous = previous;
		_comparer = comparer;
	}

	public bool Get(IsExactInput parameter)
	{
		var (source, destination) = parameter;

		return _previous.Get(parameter)
		       && destination.GetKeys()
		                     .Select(k => k.Properties.Select(p => p.Name).ToHashSet())
		                     .ToHashSet(_comparer)
		                     .SetEquals(source.GetKeys()
		                                      .Select(k => k.Properties.Select(p => p.Name).ToHashSet())
		                                      .ToHashSet(_comparer))
		       && source.GetIndexes()
		                .Select(i => i.Properties.Select(p => p.Name).ToHashSet())
		                .ToHashSet(_comparer)
		                .SetEquals(destination.GetIndexes()
		                                      .Select(i => i.Properties.Select(p => p.Name).ToHashSet())
		                                      .ToHashSet(_comparer));
	}
}