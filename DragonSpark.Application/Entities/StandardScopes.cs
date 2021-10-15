using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities;

public class StandardScopes<T> : IStandardScopes where T : DbContext
{
	readonly IContexts<T> _contexts;

	public StandardScopes(IContexts<T> contexts) => _contexts = contexts;

	public Scope Get()
	{
		var context = _contexts.Get();
		return new(context, new Boundary(context));
	}

	sealed class Boundary : DragonSpark.Model.Operations.Instance<IDisposable>, IBoundary
	{
		public Boundary(IDisposable instance) : base(instance) {}
	}
}