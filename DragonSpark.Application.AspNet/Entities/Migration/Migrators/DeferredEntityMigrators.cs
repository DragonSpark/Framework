using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class DeferredEntityMigrators : IEntityMigrators
{
	readonly IResult<IEntityMigrators> _previous;

	public DeferredEntityMigrators(Func<IEntityMigrators> source) : this(source.Start().Singleton().Get()) {}

	public DeferredEntityMigrators(IResult<IEntityMigrators> previous) => _previous = previous;

	public Array<IEntityMigrator> Get(MigrationInput parameter) => _previous.Get().Get(parameter);
}