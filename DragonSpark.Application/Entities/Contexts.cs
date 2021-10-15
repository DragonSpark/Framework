using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class Contexts<T> : IContexts<T> where T : DbContext
{
	readonly IDbContextFactory<T> _factory;

	public Contexts(IDbContextFactory<T> factory) => _factory = factory;

	public T Get() => _factory.CreateDbContext();
}