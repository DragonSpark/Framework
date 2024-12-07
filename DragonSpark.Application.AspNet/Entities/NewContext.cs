using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class NewContext<T> : INewContext<T> where T : DbContext
{
	readonly IDbContextFactory<T> _factory;

	public NewContext(IDbContextFactory<T> factory) => _factory = factory;

	public T Get() => _factory.CreateDbContext();
}