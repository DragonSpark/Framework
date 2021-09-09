using DragonSpark.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Testing.Objects.Entities
{
	public class PooledContexts<T> : Contexts<T> where T : DbContext
	{
		public PooledContexts(DbContextOptions<T> options) : this(new PooledDbContextFactory<T>(options)) {}

		public PooledContexts(IDbContextFactory<T> factory) : base(factory) {}
	}
}