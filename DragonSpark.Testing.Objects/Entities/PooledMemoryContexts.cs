using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class PooledMemoryContexts<T> : PooledContexts<T> where T : DbContext
{
	public PooledMemoryContexts() : base(NewMemoryOptions<T>.Default.Get()) {}
}