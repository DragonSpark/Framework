using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	class Class3 {}

	public class Session<TIn, TContext, TOut, TSave> : Session<TIn, TOut, TSave> where TContext : DbContext
	{
		protected Session(TContext context, IForming<TIn, TOut?> @select, IOperation<In<TSave>> apply)
			: base(context, @select, apply) {}

		protected Session(TContext context, IQuery<TIn, TOut> select, IOperation<In<TSave>> apply)
			: base(context, select.Then().Form.SingleOrDefault(), apply) {}
	}

	public interface ISession<in TIn, TOut, in TSave> : ISelecting<TIn, TOut?>, IOperation<TSave>, IAsyncDisposable {}

	public class Session<TIn, TOut, TSave> : ISession<TIn, TOut, TSave>
	{
		readonly DbContext                  _context;
		readonly ISelecting<In<TIn>, TOut?> _select;
		readonly IOperation<In<TSave>>      _apply;

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

	public sealed class FormedAdapter<T> : IOperation<T>
	{
		readonly DbContext  _context;
		readonly IFormed<T> _operation;

		public FormedAdapter(DbContext context, IFormed<T> operation)
		{
			_context   = context;
			_operation = operation;
		}

		public ValueTask Get(T parameter) => _operation.Get(new In<T>(_context, parameter));
	}


	public sealed class FormingAdapter<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly DbContext          _instance;
		readonly IForming<TIn, TOut> _forming;

		public FormingAdapter(DbContext instance, IForming<TIn, TOut> forming)
		{
			_instance = instance;
			_forming   = forming;
		}

		public ValueTask<TOut> Get(TIn parameter) => _forming.Get(new(_instance, parameter));
	}
}