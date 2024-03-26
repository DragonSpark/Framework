using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class Scopes<T> : IScopes where T : DbContext
{
	readonly INewContext<T> _new;

	public Scopes(INewContext<T> @new) => _new = @new;

	public Scope Get() => new(_new.Get());
}