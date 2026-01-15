using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class CompareNavigations : ISelect<ComparisonInput, NavigationComparison>
{
	public static CompareNavigations Default { get; } = new();

	CompareNavigations() {}

	public NavigationComparison Get(ComparisonInput parameter)
	{
		var (from, to) = parameter;
		var added   = to.Navigations.Set.Except(from.Navigations.Set).ToArray();
		var removed = from.Navigations.Set.Except(to.Navigations.Set).ToArray();
		var changed = from.Navigations.Set.Intersect(to.Navigations.Set)
		                  .Where(n => from.Navigations.Map[n.Name].TargetType != to.Navigations.Map[n.Name].TargetType)
		                  .ToArray();
		return new(added, removed, changed);
	}
}