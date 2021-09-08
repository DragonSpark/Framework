using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public interface IQueries<T> : IResulting<Session<T>> {}

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

		public async ValueTask<Session<TOut>> Get()
		{
			var (subject, session) = _invocations.Get();
			var query      = _compiled(subject, _parameter);
			var disposable = await session.Await();
			return new(query, disposable);
		}
	}

	sealed class Compiled<TIn, TOut> : ISelect<TIn, IQueries<TOut>>
	{
		readonly IInvocations                           _invocations;
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		public Compiled(IInvocations invocations, Func<DbContext, TIn, IQueryable<TOut>> compiled)
		{
			_invocations = invocations;
			_compiled    = compiled;
		}

		public IQueries<TOut> Get(TIn parameter) => new Queries<TIn, TOut>(_invocations, parameter, _compiled);
	}

	public interface ISession : IResulting<IDisposable> {}

	public readonly struct Session<T> : IDisposable
	{
		readonly IDisposable _session;

		public Session(IQueryable<T> subject, IDisposable session)
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
		public ScopedInvocation(DbContext context)
			: this(new Invocation(context, new ScopedSession(Locks.Default.Get(context)))) {}

		public ScopedInvocation(Invocation instance) : base(instance) {}
	}

	sealed class ScopedSession : ISession
	{
		readonly AsyncLock _lock;

		public ScopedSession(AsyncLock @lock) => _lock = @lock;

		public async ValueTask<IDisposable> Get() => new Session(await _lock.LockAsync());

		sealed class Session : IDisposable
		{
			readonly AsyncLock.Releaser _disposable;

			public Session(AsyncLock.Releaser disposable) => _disposable = disposable;

			public void Dispose()
			{
				var disposable = _disposable;
				disposable.Dispose();
			}
		}
	}

	public readonly record struct Invocation(DbContext Subject, ISession Disposable);
}