using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class CompareProperties : ISelect<ComparisonInput, PropertyComparison>
{
	public static CompareProperties Default { get; } = new();

	CompareProperties() {}

	public PropertyComparison Get(ComparisonInput parameter)
	{
		var (from, to) = parameter;
		var added   = to.Properties.Set.Except(from.Properties.Set).ToArray();
		var removed = from.Properties.Set.Except(to.Properties.Set).ToArray();
		var changed = from.Properties.Set.Intersect(to.Properties.Set)
		                  .Where(p => from.Properties.Map[p.Name].Type != to.Properties.Map[p.Name].Type)
		                  .ToArray();
		return new(added, removed, changed);
	}
}