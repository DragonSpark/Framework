using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

public sealed class Source<T> : ISource<T>
{
	public static Source<T> Default { get; } = new();

	Source() : this(SourceQueries<T>.Default) {}

	readonly ISelect<IModel, IAlteration<IQueryable<T>>> _queries;

	public Source(ISelect<IModel, IAlteration<IQueryable<T>>> queries) => _queries = queries;

	public IQueryable<T> Get(Stop<SourceInput<T>> parameter)
	{
		var ((_, (source, _), queryable, _, _, _), _) = parameter;
		return _queries.Get(source.Model).Get(queryable);
	}
}