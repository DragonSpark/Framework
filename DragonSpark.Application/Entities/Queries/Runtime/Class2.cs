using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime
{
	class Class2 {}


	public class RuntimeQuery<TIn, TContext, TOut> : RuntimeQuery<TIn, TOut> where TContext : DbContext
	{
		protected RuntimeQuery(IContexts<TContext> contexts, IQuery<TIn, TOut> query)
			: base(new Invocations<TContext>(contexts), query) {}
	}

	sealed class CompileAdapter<TIn, TOut> : ISelect<(DbContext, TIn), IQueryable<TOut>>
	{
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		public CompileAdapter(Func<DbContext, TIn, IQueryable<TOut>> compiled) => _compiled = compiled;

		public IQueryable<TOut> Get((DbContext, TIn) parameter) => _compiled(parameter.Item1, parameter.Item2);
	}

	public interface IRuntimeQuery<in TIn, TOut> : ISelect<TIn, IQueries<TOut>> {}

	public class RuntimeQuery<TIn, TOut> : IRuntimeQuery<TIn, TOut>
	{
		readonly IInvocations                           _invocations;
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		public RuntimeQuery(DbContext context, IQuery<TIn, TOut> query)
			: this(new ScopedInvocation(context), query) {}

		public RuntimeQuery(IInvocations invocations, IQuery<TIn, TOut> query)
			: this(invocations, query.Get().Expand().Compile()) {}

		RuntimeQuery(IInvocations invocations, Func<DbContext, TIn, IQueryable<TOut>> compiled)
		{
			_invocations = invocations;
			_compiled    = compiled;
		}

		public IQueries<TOut> Get(TIn parameter) => new Queries<TIn, TOut>(_invocations, parameter, _compiled);
	}
}