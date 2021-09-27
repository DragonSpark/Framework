using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime
{
	public class RuntimeQuery<T> : RuntimeQuery<None, T>, IRuntimeQuery<T>
	{
		protected RuntimeQuery(DbContext context, IQuery<None, T> query) : base(context, query) {}

		public RuntimeQuery(IScopes scopes, IQuery<None, T> query) : base(scopes, query) {}
	}
	public class RuntimeQuery<TIn, TOut> : IRuntimeQuery<TIn, TOut>
	{
		readonly IScopes                                _scopes;
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		protected RuntimeQuery(DbContext context, IQuery<TIn, TOut> query)
			: this(new SessionScopes(context), query) {}

		public RuntimeQuery(IScopes scopes, IQuery<TIn, TOut> query)
			: this(scopes, query.Get().Expand().Compile()) {}

		RuntimeQuery(IScopes scopes, Func<DbContext, TIn, IQueryable<TOut>> compiled)
		{
			_scopes   = scopes;
			_compiled = compiled;
		}

		public IQueries<TOut> Get(TIn parameter) => new Queries<TIn, TOut>(_scopes, parameter, _compiled);
	}

}