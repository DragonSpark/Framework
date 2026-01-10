using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct IsExactInput(IEntityType Source, IEntityType Destination);