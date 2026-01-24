using DragonSpark.Model;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public static class Extensions
{
	public static EntityMigratorRegistration Registered(this IEntityMigrator @this) => new(@this);
}

public static class Ignoring
{
	public static EntityMigratorRegistration Type<T>() => new(new EmptyEntityMigrator(typeof(T)));
}

// TODO

public sealed class EmptyEntityMigrator : Instance<EntityTypeMapping>, IEntityMigrator
{
	public EmptyEntityMigrator(Type from) : base(new(from, typeof(None))) {}

	public void Execute(EntityPreMigrationInput parameter) {}

	public void Execute(EntityPostMigrationInput parameter) {}

	public void Execute(EntityMigratorInput parameter) {}
}