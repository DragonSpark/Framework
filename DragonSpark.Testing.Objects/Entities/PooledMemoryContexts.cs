using DragonSpark.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class PooledMemoryContexts<T> : Contexts<T> where T : DbContext
	{
		public PooledMemoryContexts() : this(NewMemoryOptions<T>.Default.Get()) {}

		public PooledMemoryContexts(DbContextOptions<T> options) : this(new PooledDbContextFactory<T>(options)) {}

		public PooledMemoryContexts(IDbContextFactory<T> factory) : base(factory) {}
	}
}