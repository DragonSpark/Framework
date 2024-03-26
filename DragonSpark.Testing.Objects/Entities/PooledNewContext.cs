using DragonSpark.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Testing.Objects.Entities;

public class PooledNewContext<T> : NewContext<T> where T : DbContext
{
	public PooledNewContext(DbContextOptions<T> options) : this(new PooledDbContextFactory<T>(options)) {}

	public PooledNewContext(IDbContextFactory<T> factory) : base(factory) {}
}