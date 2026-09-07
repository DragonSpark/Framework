using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct ConstructEntityMigratorInput(
	DbContext Source,
	IDestination Destination,
	IEntityType From,
	IEntityType To);