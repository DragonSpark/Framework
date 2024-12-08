using DragonSpark.Model.Sequences;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled;

readonly record struct RewriteResult(LambdaExpression Expression, Array<Type> Types, Array<Delegate> Delegates);