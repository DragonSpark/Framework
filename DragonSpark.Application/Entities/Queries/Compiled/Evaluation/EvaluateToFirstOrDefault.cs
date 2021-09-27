using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToFirstOrDefault<T> : EvaluateToFirstOrDefault<None, T>
	{
		protected EvaluateToFirstOrDefault(IScopes scopes,
		                                   Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(scopes, expression.Then()) {}

		protected EvaluateToFirstOrDefault(IScopes scopes,
		                                   Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(scopes, expression) {}

		protected EvaluateToFirstOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirstOrDefault<TIn, T> : Evaluate<TIn, T, T?>
	{
		protected EvaluateToFirstOrDefault(IScopes scopes,
		                                   Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(scopes, expression)) {}

		protected EvaluateToFirstOrDefault(IInvoke<TIn, T> invoke) : base(invoke, ToFirstOrDefault<T>.Default) {}
	}
}