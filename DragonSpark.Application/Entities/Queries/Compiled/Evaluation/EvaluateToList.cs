using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToList<T> : EvaluateToList<None, T>
	{
		public EvaluateToList(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(scopes, expression.Then()) {}

		public EvaluateToList(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(scopes, expression) {}

		public EvaluateToList(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToList<TIn, T> : Evaluate<TIn, T, List<T>>
	{
		protected EvaluateToList(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(scopes, expression)) {}

		protected EvaluateToList(IInvoke<TIn, T> invoke) : base(invoke, ToList<T>.Default) {}
	}
}