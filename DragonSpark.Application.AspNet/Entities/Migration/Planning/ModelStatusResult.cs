using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct ModelStatusResult(FoundModelsResult Found, ImmutableArray<IEntityType> Missing);