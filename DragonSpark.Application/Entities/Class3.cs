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
		protected Session(IContexts<TContext> contexts, IQuery<TIn, TOut> select, IOperation<In<TSave>> apply)
			: this(contexts.Get(), @select, apply) {}

		protected Session(IContexts<TContext> contexts, IFormed<TIn, TOut?> @select, IOperation<In<TSave>> apply)
			: this(contexts.Get(), @select, apply) {}

		protected Session(DbContext context, IFormed<TIn, TOut?> @select, IOperation<In<TSave>> apply)
			: base(context, @select, apply) {}

		protected Session(DbContext context, IQuery<TIn, TOut> select, IOperation<In<TSave>> apply)
			: base(context, select.Then().Form.SingleOrDefault(), apply) {}
	}

	public interface ISession<in TIn, TOut, in TSave> : ISelecting<TIn, TOut?>, IOperation<TSave>, IAsyncDisposable {}

	public class Session<TIn, TOut, TSave> : DragonSpark.Model.Results.Instance<DbContext>, ISession<TIn, TOut, TSave>
	{
		readonly DbContext                  _context;
		readonly ISelecting<In<TIn>, TOut?> _select;
		readonly IOperation<In<TSave>>      _apply;

		protected Session(DbContext context, IFormed<TIn, TOut?> select, IOperation<In<TSave>> apply) : base(context)
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

	public sealed class Formed<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly DbContext          _instance;
		readonly IFormed<TIn, TOut> _formed;

		public Formed(DbContext instance, IFormed<TIn, TOut> formed)
		{
			_instance = instance;
			_formed   = formed;
		}

		public ValueTask<TOut> Get(TIn parameter) => _formed.Get(new(_instance, parameter));
	}
}