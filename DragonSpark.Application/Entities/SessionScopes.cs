using AsyncUtilities;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public interface IFactoryScopes : IScopes {}

	public interface ISessionScopes : IScopes {}

	public class FactoryScopes<T> : IFactoryScopes where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public FactoryScopes(IContexts<T> contexts) => _contexts = contexts;

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

	public class SessionScopes : DragonSpark.Model.Results.Instance<Scope>, ISessionScopes
	{
		public SessionScopes(DbContext context) : this(context, Locks.Default.Get(context)) {}

		public SessionScopes(DbContext context, AsyncLock @lock) : this(new Scope(context, new Boundary(@lock))) {}

		public SessionScopes(Scope instance) : base(instance) {}

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