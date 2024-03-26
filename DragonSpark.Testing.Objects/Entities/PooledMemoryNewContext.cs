using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class PooledMemoryNewContext<T> : PooledNewContext<T> where T : DbContext
{
	public PooledMemoryNewContext() : base(NewMemoryOptions<T>.Default.Get()) {}
}