namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct NavigationRecord(string Name, string TargetType, bool IsCollection, bool IsOnDependent);