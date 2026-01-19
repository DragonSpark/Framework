using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct SaveBatchInput<T>(DbContext Destination, Lease<T> Entities) where T : class;