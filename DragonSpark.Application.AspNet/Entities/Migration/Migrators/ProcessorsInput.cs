using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct ProcessorsInput(DbContext Source, DbContext Destination, IMap Map);