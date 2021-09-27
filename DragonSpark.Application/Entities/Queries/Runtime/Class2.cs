using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime
{
	class Class2 {}

	public class ContextRuntimeQuery<TContext, TOut> : RuntimeQuery<TOut> where TContext : DbContext
	{
		protected ContextRuntimeQuery(IContexts<TContext> contexts, IQuery<TOut> query)
			: base(new Scopes<TContext>(contexts), query) {}
	}

	public class ContextRuntimeQuery<TIn, TContext, TOut> : RuntimeQuery<TIn, TOut> where TContext : DbContext
	{
		protected ContextRuntimeQuery(IContexts<TContext> contexts, IQuery<TIn, TOut> query)
			: base(new Scopes<TContext>(contexts), query) {}
	}

	sealed class CompileAdapter<TIn, TOut> : ISelect<(DbContext, TIn), IQueryable<TOut>>
	{
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		public CompileAdapter(Func<DbContext, TIn, IQueryable<TOut>> compiled) => _compiled = compiled;

		public IQueryable<TOut> Get((DbContext, TIn) parameter) => _compiled(parameter.Item1, parameter.Item2);
	}

	public interface IRuntimeQuery<T> : IRuntimeQuery<None, T> {}

	public interface IRuntimeQuery<in TIn, TOut> : ISelect<TIn, IQueries<TOut>> {}

	public class RuntimeQuery<T> : RuntimeQuery<None, T>, IRuntimeQuery<T>
	{
		protected RuntimeQuery(DbContext context, IQuery<None, T> query) : base(context, query) {}

		public RuntimeQuery(IScopes scopes, IQuery<None, T> query) : base(scopes, query) {}
	}

	public class RuntimeQuery<TIn, TOut> : IRuntimeQuery<TIn, TOut>
	{
		readonly IScopes                           _scopes;
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		protected RuntimeQuery(DbContext context, IQuery<TIn, TOut> query)
			: this(new Scopes(context), query) {}

		public RuntimeQuery(IScopes scopes, IQuery<TIn, TOut> query)
			: this(scopes, query.Get().Expand().Compile()) {}

		RuntimeQuery(IScopes scopes, Func<DbContext, TIn, IQueryable<TOut>> compiled)
		{
			_scopes = scopes;
			_compiled    = compiled;
		}

		public IQueries<TOut> Get(TIn parameter) => new Queries<TIn, TOut>(_scopes, parameter, _compiled);
	}
}