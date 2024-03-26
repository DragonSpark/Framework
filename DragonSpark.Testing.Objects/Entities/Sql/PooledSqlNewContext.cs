using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities.Sql;

[UsedImplicitly]
public sealed class PooledSqlNewContext<T> : PooledNewContext<T> where T : DbContext
{
	public PooledSqlNewContext() : base(NewSqlOptions<T>.Default.Get()) {}
}