using DragonSpark.Application.AspNet.Entities.Migration.Migrators;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public readonly record struct BuildStoreInput<T>(Workspace Workspace, IReadOnlyCollection<T> Source);