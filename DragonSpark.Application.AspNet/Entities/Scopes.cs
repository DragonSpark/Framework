using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public class Scopes<T> : IScopes where T : DbContext
{
	readonly INewContext<T> _new;

	public Scopes(INewContext<T> @new) => _new = @new;

	[MustDisposeResource]
	public Scope Get() => new(_new.Get());
}
