using DragonSpark.Model.Sequences;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compilation
{
	readonly record struct RewriteResult(LambdaExpression Expression, Array<Type> Types, Array<Delegate> Delegates);
}