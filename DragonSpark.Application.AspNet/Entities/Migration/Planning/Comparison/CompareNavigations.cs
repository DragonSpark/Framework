using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class CompareNavigations : ISelect<CompareNavigationsInput, NavigationComparison>
{
	readonly IEqualityComparer<NavigationRecord> _comparer;
	readonly IEqualityComparer<IEntityType>      _types;

	public CompareNavigations(IEntityTypes types) : this(new LocationAwareEntityTypeEqualityComparer(types)) {}

	public CompareNavigations(IEqualityComparer<IEntityType> types)
		: this(new NavigationRecordEqualityComparer(types), types) {}

	public CompareNavigations(IEqualityComparer<NavigationRecord> comparer, IEqualityComparer<IEntityType> types)
	{
		_comparer = comparer;
		_types    = types;
	}

	public NavigationComparison Get(CompareNavigationsInput parameter)
	{
		var (from, to) = parameter;
		var added   = to.Set.Except(from.Set, _comparer).ToArray();
		var removed = from.Set.Except(to.Set, _comparer).ToArray();
		var changed = from.Set.Intersect(to.Set, _comparer)
		                  .Where(x => !_types.Equals(from.Map[x.Name].Type, to.Map[x.Name].Type))
		                  .ToArray();
		return new(added, removed, changed);
	}
}