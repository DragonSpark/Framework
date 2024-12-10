using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public class NewContext<T> : INewContext<T> where T : DbContext
{
	readonly IDbContextFactory<T> _factory;

	public NewContext(IDbContextFactory<T> factory) => _factory = factory;

	[MustDisposeResource]
	public T Get() => _factory.CreateDbContext();
}
