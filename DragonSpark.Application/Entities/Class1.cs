using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	class Class1 {}

	public readonly struct SaveOperation<T> : IAsyncDisposable where T : DbContext
	{
		public SaveOperation(T subject) => Subject = subject;

		public T Subject { get; }

		public async ValueTask DisposeAsync()
		{
			await Subject.SaveChangesAsync().ConfigureAwait(false);
			await Subject.DisposeAsync().ConfigureAwait(false);
		}
	}

	public interface ISaveContext<T> : IResult<SaveOperation<T>> where T : DbContext {}

	public class SaveContext<T> : ISaveContext<T> where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public SaveContext(IContexts<T> contexts) => _contexts = contexts;

		public SaveOperation<T> Get() => new(_contexts.Get());
	}

	public class Modify<TContext, T> : IOperation<T> where TContext : DbContext
	{
		readonly ISaveContext<TContext> _save;
		readonly Action<Modify<T>>      _configure;

		protected Modify(ISaveContext<TContext> save, ICommand<Modify<T>> modification)
			: this(save, modification.Execute) {}

		protected Modify(ISaveContext<TContext> save, Action<Modify<T>> configure)
		{
			_save      = save;
			_configure = configure;
		}

		public async ValueTask Get(T parameter)
		{
			await using var save = _save.Get();
			_configure(new (save.Subject, parameter));
		}
	}

	public interface IModification<T> : ICommand<Modify<T>> {}

	public readonly record struct Modify<T>(DbContext Context, T Parameter);

	public class RemoveEntity<TIn, TContext, T> : Operation<TIn> where TContext : DbContext where T : class
	{
		protected RemoveEntity(IContexts<TContext> contexts, IQuery<TIn, T> query, RemoveEntity<TContext, T> remove)
			: this(contexts.Then().Use(query).To.Single(), remove) {}

		protected RemoveEntity(ISelecting<TIn, T> entity, RemoveEntity<TContext, T> remove)
			: base(entity.Then().Terminate(remove)) {}
	}

	public class RemoveEntity<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		public RemoveEntity(ISaveContext<TContext> contexts)
			: base(contexts, x => x.Context.Set<T>().Remove(x.Parameter)){}
	}

}