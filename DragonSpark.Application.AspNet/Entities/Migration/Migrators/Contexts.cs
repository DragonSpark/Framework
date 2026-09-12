using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public record Contexts(IEntityType SourceType, IModel Destination);

public record Contexts<T>(
	Func<DbContext, IQueryable<T>> Query,
	IEntityType SourceType,
	IModel Destination)
	: Contexts(SourceType, Destination) where T : class
{
	public Contexts(DbContext source, IModel destination) : this(source.Set<T>().EntityType, destination) {}

	public Contexts(IEntityType SourceType, IModel Destination)
		: this(d => d.Set<T>().Exact(), SourceType, Destination) {}
}