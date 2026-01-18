using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record ExactEntityComparisonResult(IEntityType From, IEntityType To)
	: MatchedEntityComparisonResult(From, To);