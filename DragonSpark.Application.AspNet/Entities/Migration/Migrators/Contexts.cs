using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public record Contexts(DbContext Source, DbContext Destination, IEntityType From);

public record Contexts<T>(DbContext Source, DbContext Destination, IEntityType From, IQueryable<T> Subject)
	: Contexts(Source, Destination, From) where T : class
{
	public Contexts(DbContext Source, DbContext Destination, DbSet<T> subject)
		: this(Source, Destination, subject.EntityType, subject.Exact()) {}

	public Contexts(DbContext Source, DbContext Destination) : this(Source, Destination, Source.Set<T>()) {}

	public Contexts(DbContext Source, DbContext Destination, string name)
		: this(Source, Destination, Source.Set<T>(name)) {}
}