using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class Contexts<T> : IContexts where T : DbContext
{
	readonly INewContext<T> _new;

	public Contexts(INewContext<T> @new) => _new = @new;

	public DbContext Get() => _new.Get();
}