using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

public sealed class Source<T> : ISource<T>
{
	public static Source<T> Default { get; } = new();

	Source() {}

	public IQueryable<T> Get(Stop<SourceInput<T>> parameter) => parameter.Subject.From;
}