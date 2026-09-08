using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

public readonly record struct Entry<T>(T Instance, PropertyValues? Values);