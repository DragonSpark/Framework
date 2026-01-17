using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record EntityComparisonResult(IEntityType From, IEntityType To, EntityModifications Modifications);