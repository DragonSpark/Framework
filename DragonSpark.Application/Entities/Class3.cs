using AsyncUtilities;
using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public interface IBoundary : IResulting<IDisposable> {}

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
				             ? new Invocation(provider.GetRequiredService<DbContext>(), EmptyBoundary.Default)
				             : _previous.Get();
			return result;
		}
	}

	sealed class EmptyBoundary : DragonSpark.Model.Operations.Instance<IDisposable>, IBoundary
	{
		public static EmptyBoundary Default { get; } = new();

		EmptyBoundary() : base(EmptyDisposable.Default) {}
	}

	public interface IInvocations : IResult<Invocation> {}

	sealed class Invocations<T> : IInvocations where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public Invocations(IContexts<T> contexts) => _contexts = contexts;

		public Invocation Get()
		{
			var context = _contexts.Get();
			return new(context, new Boundary(context));
		}

		sealed class Boundary : DragonSpark.Model.Operations.Instance<IDisposable>, IBoundary
		{
			public Boundary(IDisposable instance) : base(instance) {}
		}
	}

	sealed class ScopedInvocation : DragonSpark.Model.Results.Instance<Invocation>, IInvocations
	{
		public ScopedInvocation(DbContext context) : this(context, Locks.Default.Get(context)) {}

		public ScopedInvocation(DbContext context, AsyncLock @lock)
			: this(new Invocation(context, new Boundary(@lock))) {}

		public ScopedInvocation(Invocation instance) : base(instance) {}

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

	public readonly record struct Invocation(DbContext Subject, IBoundary Boundary);
}