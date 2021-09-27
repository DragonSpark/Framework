using DragonSpark.Model;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToArray<T> : EvaluateToArray<None, T>
	{
		public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(scopes, expression.Then()) {}

		public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(scopes, expression) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToArray<TIn, T> : Evaluate<TIn, T, Array<T>>
	{
		public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(scopes, expression)) {}

		protected EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}
}