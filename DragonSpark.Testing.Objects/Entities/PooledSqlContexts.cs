using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class PooledSqlContexts<T> : PooledContexts<T> where T : DbContext
	{
		public PooledSqlContexts() : base(NewSqlOptions<T>.Default.Get()) {}
	}
}