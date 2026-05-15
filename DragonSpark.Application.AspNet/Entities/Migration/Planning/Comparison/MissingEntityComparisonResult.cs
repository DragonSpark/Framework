using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record MissingEntityComparisonResult(IEntityType From) : EntityComparisonResult(From);