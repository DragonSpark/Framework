using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public record Contexts(DbContext Source, DbContext Destination);

public sealed record Contexts<T>(DbContext Source, DbContext Destination, IQueryable<T> Subject)
	: Contexts(Source, Destination) where T : class
{
	public Contexts(DbContext Source, DbContext Destination)
		: this(Source, Destination, Source.Set<T>().AsNoTracking()) {}
}