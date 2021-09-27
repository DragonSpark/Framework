using AsyncUtilities;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class Scopes<T> : IScopes where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public Scopes(IContexts<T> contexts) => _contexts = contexts;

		public Scope Get()
		{
			var context = _contexts.Get();
			return new(context, new Boundary(context));
		}

		sealed class Boundary : Instance<IDisposable>, IBoundary
		{
			public Boundary(IDisposable instance) : base(instance) {}
		}
	}

	sealed class Scopes : DragonSpark.Model.Results.Instance<Scope>, IScopes
	{
		public Scopes(DbContext context) : this(context, Locks.Default.Get(context)) {}

		public Scopes(DbContext context, AsyncLock @lock) : this(new Scope(context, new Boundary(@lock))) {}

		public Scopes(Scope instance) : base(instance) {}

		sealed class Boundary : IBoundary
		{
			readonly AsyncLock _lock;

			public Boundary(AsyncLock @lock) => _lock = @lock;

			public async ValueTask<IDisposable> Get() => new Instance(await _lock.LockAsync());

			sealed class Instance : Disposable<AsyncLock.Releaser>
			{
				public Instance(AsyncLock.Releaser disposable) : base(disposable) {}
			}
		}
	}

}