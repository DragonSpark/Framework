using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct FoundModelsResult(
	ImmutableArray<ExactEntityComparisonResult> Exact,
	IReadOnlyCollection<ModifiedEntityComparisonResult> Modified);