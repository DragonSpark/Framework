using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime
{
	class Class1 {}

	public interface IQueries<T> : IResulting<Query<T>> {}

	sealed class Queries<TIn, TOut> : IQueries<TOut>
	{
		readonly IInvocations                           _invocations;
		readonly TIn                                    _parameter;
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		public Queries(IInvocations invocations, TIn parameter, Func<DbContext, TIn, IQueryable<TOut>> compiled)
		{
			_invocations = invocations;
			_parameter   = parameter;
			_compiled    = compiled;
		}

		public async ValueTask<Query<TOut>> Get()
		{
			var (subject, session) = _invocations.Get();
			var query      = _compiled(subject, _parameter);
			var disposable = await session.Await();
			return new(query, disposable);
		}
	}

	public interface ISession : IResulting<IDisposable> {}

	public readonly struct Query<T> : IDisposable
	{
		readonly IDisposable _session;

		public Query(IQueryable<T> subject, IDisposable session)
		{
			_session = session;
			Subject  = subject;
		}

		public IQueryable<T> Subject { get; }

		public void Dispose()
		{
			_session.Dispose();
		}
	}

	sealed class AmbientAwareInvocations : IInvocations
	{
		readonly IInvocations               _previous;
		readonly IResult<IServiceProvider?> _provider;

		public AmbientAwareInvocations(IInvocations previous) : this(previous, AmbientProvider.Default) {}

		public AmbientAwareInvocations(IInvocations previous, IResult<IServiceProvider?> provider)
		{
			_previous = previous;
			_provider = provider;
		}

		public Invocation Get()
		{
			var provider = _provider.Get();
			var result = provider != null
				             ? new Invocation(provider.GetRequiredService<DbContext>(), EmptySession.Default)
				             : _previous.Get();
			return result;
		}
	}

	sealed class EmptySession : DragonSpark.Model.Operations.Instance<IDisposable>, ISession
	{
		public static EmptySession Default { get; } = new();

		EmptySession() : base(EmptyDisposable.Default) {}
	}

	public interface IInvocations : IResult<Invocation> {}

	sealed class Invocations<T> : IInvocations where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public Invocations(IContexts<T> contexts) => _contexts = contexts;

		public Invocation Get()
		{
			var context = _contexts.Get();
			return new Invocation(context, new Session(context));
		}

		sealed class Session : DragonSpark.Model.Operations.Instance<IDisposable>, ISession
		{
			public Session(IDisposable instance) : base(instance) {}
		}
	}

	sealed class ScopedInvocation : DragonSpark.Model.Results.Instance<Invocation>, IInvocations
	{
		public ScopedInvocation(DbContext context) : this(context, Locks.Default.Get(context)) {}

		public ScopedInvocation(DbContext context, AsyncLock @lock)
			: this(new Invocation(context, new Session(@lock))) {}

		public ScopedInvocation(Invocation instance) : base(instance) {}

		sealed class Session : ISession
		{
			readonly AsyncLock _lock;

			public Session(AsyncLock @lock) => _lock = @lock;

			public async ValueTask<IDisposable> Get() => new Instance(await _lock.LockAsync());

			sealed class Instance : Disposable<AsyncLock.Releaser>
			{
				public Instance(AsyncLock.Releaser disposable) : base(disposable) {}
			}
		}
	}

	public readonly record struct Invocation(DbContext Subject, ISession Disposable);
}