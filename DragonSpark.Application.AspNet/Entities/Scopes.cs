using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public sealed class Scopes<T> : IScopes where T : DbContext
{
	readonly INewContext<T> _new;

	public Scopes(INewContext<T> @new) => _new = @new;

	[MustDisposeResource]
	public Scope Get() => new(_new.Get());
}

public sealed class Scopes : Instance<Scope>, IScopes
{
	public Scopes(DbContext instance) : this(instance, EmptyDisposable.Default) {}

	// ReSharper disable once NotDisposedResource
	public Scopes(DbContext instance, IDisposable disposable) : base(new(instance, disposable)) {}
}
