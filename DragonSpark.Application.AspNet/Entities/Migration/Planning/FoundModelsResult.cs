using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct FoundModelsResult(
	IReadOnlyCollection<IEntityType> Exact,
	IReadOnlyCollection<EntityComparisonResult> Modified);