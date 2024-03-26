using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class StandardScopes<T> : IStandardScopes where T : DbContext
{
	readonly IContexts<T> _contexts;

	public StandardScopes(IContexts<T> contexts) => _contexts = contexts;

	public DbContext Get() => _contexts.Get();
}