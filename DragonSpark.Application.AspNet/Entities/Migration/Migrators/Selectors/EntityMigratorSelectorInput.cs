using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

public readonly record struct EntityMigratorSelectorInput(
	DbContext Source,
	DbContext Destination,
	EntityComparisonResult Result);