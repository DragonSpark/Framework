using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public abstract record MatchedEntityComparisonResult(IEntityType From, IEntityType To) : EntityComparisonResult(From);