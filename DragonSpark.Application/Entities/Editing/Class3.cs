using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	class Class3 {}

	public class Session<TIn, TContext, TOut, TSave> : Session<TIn, TOut, TSave> where TContext : DbContext
	{
		protected Session(TContext context, IQuery<TIn, TOut> select, IOperation<In<TSave>> apply)
			: base(context, select.Then().Form.SingleOrDefault(), apply) {}

		protected Session(TContext context, ISessionBody<TIn, TOut, TSave> body) : base(context, body) {}

		protected Session(TContext context, IForming<TIn, TOut?> select, IOperation<In<TSave>> apply)
			: base(context, select, apply) {}
	}

	public interface ISession<in TIn, TOut, in TSave> : ISelecting<TIn, TOut?>, IOperation<TSave>, IAsyncDisposable {}

	public interface ISessionBody<TIn, TOut, TSave> : IForming<TIn, TOut?>, IFormed<TSave> {}

	public class SessionBody<TIn, TOut, TSave> : ISessionBody<TIn, TOut, TSave>
	{
		readonly ISelecting<In<TIn>, TOut?> _select;
		readonly IOperation<In<TSave>>      _save;

		public SessionBody(ISelecting<In<TIn>, TOut?> select, IOperation<In<TSave>> save)
		{
			_select = select;
			_save   = save;
		}

		public ValueTask<TOut?> Get(In<TIn> parameter) => _select.Get(parameter);

		public ValueTask Get(In<TSave> parameter) => _save.Get(parameter);
	}

	public class Session<TIn, TOut, TSave> : ISession<TIn, TOut, TSave>
	{
		readonly DbContext                  _context;
		readonly ISelecting<In<TIn>, TOut?> _select;
		readonly IOperation<In<TSave>>      _apply;

		protected Session(DbContext context, ISessionBody<TIn, TOut, TSave> body) : this(context, body, body) {}

		protected Session(DbContext context, IForming<TIn, TOut?> select, IOperation<In<TSave>> apply)
		{
			_context = context;
			_select  = select;
			_apply   = apply;
		}

		public ValueTask<TOut?> Get(TIn parameter) => _select.Get(new(_context, parameter));

		public async ValueTask Get(TSave parameter)
		{
			await _apply.Await(new(_context, parameter));
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		public ValueTask DisposeAsync() => _context.DisposeAsync();
	}

	sealed class FormedAdapter<T, TContext> : IOperation<T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IFormed<T>          _operation;

		public FormedAdapter(IContexts<TContext> contexts, IFormed<T> operation)
		{
			_contexts  = contexts;
			_operation = operation;
		}

		public async ValueTask Get(T parameter)
		{
			await using var context = _contexts.Get();
			await _operation.Await(new(context, parameter));
		}
	}

	sealed class FixedFormedAdapter<T> : IOperation<T>
	{
		readonly DbContext  _context;
		readonly IFormed<T> _operation;

		public FixedFormedAdapter(DbContext context, IFormed<T> operation)
		{
			_context   = context;
			_operation = operation;
		}

		public ValueTask Get(T parameter) => _operation.Get(new In<T>(_context, parameter));
	}

	public class FixedAdapter<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly DbContext           _instance;
		readonly IForming<TIn, TOut> _forming;

		public FixedAdapter(DbContext instance, IForming<TIn, TOut> forming)
		{
			_instance = instance;
			_forming  = forming;
		}

		public ValueTask<TOut> Get(TIn parameter) => _forming.Get(new(_instance, parameter));
	}

	/**/
}