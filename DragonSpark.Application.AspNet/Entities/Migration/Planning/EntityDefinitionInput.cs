using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct EntityDefinitionInput(IEntityType Source, IEntityType Destination);