using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record ModifiedEntityComparisonResult(
	IEntityType From,
	IEntityType To,
	EntityModifications Modifications) : MatchedEntityComparisonResult(From, To);