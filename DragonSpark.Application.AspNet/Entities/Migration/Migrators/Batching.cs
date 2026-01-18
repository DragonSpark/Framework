using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public record Batching(DbContext Source, DbContext Destination);

public sealed record Batching<T>(DbContext Source, DbContext Destination, IQueryable<T> Subject)
	: Batching(Source, Destination) where T : class
{
	public Batching(DbContext Source, DbContext Destination) : this(Source, Destination, Source.Set<T>()) {}
}