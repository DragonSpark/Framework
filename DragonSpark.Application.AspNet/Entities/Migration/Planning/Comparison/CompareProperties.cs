using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class CompareProperties : ISelect<ComparePropertiesInput, PropertyChanges>
{
	public static CompareProperties Default { get; } = new();

	public CompareProperties()
		: this(PropertyRecordEqualityComparer.Default, EntityMetadataEqualityComparer.Default) {}

	readonly IEqualityComparer<PropertyRecord> _comparer;
	readonly IEqualityComparer<Type>           _types;

	public CompareProperties(IEqualityComparer<PropertyRecord> comparer, IEqualityComparer<Type> types)
	{
		_comparer = comparer;
		_types    = types;
	}

	public PropertyChanges Get(ComparePropertiesInput parameter)
	{
		var (from, to) = parameter;
		var added   = to.Set.Except(from.Set, _comparer).ToImmutableArray();
		var removed = from.Set.Except(to.Set, _comparer).ToImmutableArray();
		var changed = from.Set.Intersect(to.Set, _comparer)
		                  .Where(x => !_types.Equals(from.Map[x.Name].Type, to.Map[x.Name].Type))
		                  .ToImmutableArray();
		return new(added, removed, changed);
	}
}