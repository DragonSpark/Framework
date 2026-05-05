using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct ProcessorsInput(IEntityType From, IMap Map);