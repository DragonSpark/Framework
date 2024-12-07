using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class ManyGeneric<TIn, TOut> : Reflection.Types.Generic<LambdaExpression, Delegate[], IElements<TIn, TOut>>
{
	public ManyGeneric(Type definition) : base(definition) {}
}