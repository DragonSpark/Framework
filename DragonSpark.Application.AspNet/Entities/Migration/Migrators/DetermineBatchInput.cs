using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct DetermineBatchInput(DbContext Source, DbContext Destination, IMap Map);