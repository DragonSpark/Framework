using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

public readonly record struct ProcessorsInput(IEntityType From, IMap Map);