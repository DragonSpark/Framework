using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class ComposeEntityComparisonResults : IComposeEntityComparisonResults
{
	readonly IModelTypes                         _types;
	readonly INewLeasing<EntityComparisonResult> _new;

	public ComposeEntityComparisonResults(IModelTypes types)
		: this(types, NewLeasing<EntityComparisonResult>.Default) {}

	public ComposeEntityComparisonResults(IModelTypes types, INewLeasing<EntityComparisonResult> @new)
	{
		_types = types;
		_new   = @new;
	}

	public Leasing<EntityComparisonResult> Get(ModelStatusInput parameter)
	{
		var (types, destination) = parameter;

		var locate   = _types.Get(destination);
		var entities = new EntityComparison(locate);
		var result   = _new.Get(types.Length);
		for (var i = 0u; i < types.Length; i++)
		{
			result.Store[i] = DetermineResult(i);
		}

		return result;

		EntityComparisonResult DetermineResult(uint i)
		{
			var from = types[i];
			var to   = locate.Get(from);
			if (to is not null && from.ClrType != to.ClrType) // TODO V2: omit dictionaries for now
			{
				var comparison = entities.Get(new(from, to));
				return comparison.Modifications.Changes > 0
					       ? new ModifiedEntityComparisonResult(from, to, comparison.Modifications)
					       : new ExactEntityComparisonResult(from, to);
			}

			return new MissingEntityComparisonResult(from);
		}
	}
}