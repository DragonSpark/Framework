using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public record Contexts(IEntityType SourceType, IModel Destination);

public record Contexts<T>(
	Func<DbContext, IQueryable<T>> Source,
	IEntityType SourceType,
	IModel Destination)
	: Contexts(SourceType, Destination) where T : class
{
	public Contexts(DbContext source, IModel destination)
		: this(d => d.Set<T>().Exact(), source.Set<T>().EntityType, destination) {}

	public Contexts(DbContext source, IModel destination, string name)
		: this(d => d.Set<T>(name).Exact(), source.Set<T>(name).EntityType, destination) {}
}