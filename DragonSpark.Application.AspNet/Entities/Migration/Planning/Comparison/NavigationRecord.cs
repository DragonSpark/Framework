using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct NavigationRecord(string Name, IEntityType Type, bool IsCollection, bool IsOnDependent);