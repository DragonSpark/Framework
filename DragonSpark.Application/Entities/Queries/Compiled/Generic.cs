using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled
{
	sealed class Generic<TIn, TOut> : Reflection.Types.Generic<LambdaExpression, Delegate[], IForm<TIn, TOut>>
	{
		public Generic(Type definition) : base(definition) {}
	}
}