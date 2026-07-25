using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Model.Sequences.Memory;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public class ModelStatus : IModelStatus
{
	readonly ILease<ModelStatusInput, EntityComparisonResult> _results;

	protected ModelStatus(IModelTypes types) : this(new ComposeEntityComparisonResults(types)) {}

	public ModelStatus(IComposeEntityComparisonResults results) => _results = results;

	public ModelStatusResult Get(ModelStatusInput parameter)
	{
		using var lease = _results.Get(parameter);

		var enumerable = lease.AsEnumerable();
		var differing  = enumerable.OfType<ModifiedEntityComparisonResult>().ToImmutableArray();
		return new(new([..enumerable.OfType<ExactEntityComparisonResult>()], differing),
		           [..enumerable.OfType<MissingEntityComparisonResult>().Select(x => x.From)]);
	}
}