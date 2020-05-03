using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericSingleton : IActivateExpressions
	{
		public static GenericSingleton Default { get; } = new GenericSingleton();

		GenericSingleton() : this(typeof(Singletons).GetRuntimeMethod(nameof(Singletons.Get),
		                                                              typeof(Type).Yield().ToArray())!,
		                          new ArrayInstance<ParameterExpression>(Array<ParameterExpression>.Empty)) {}

		readonly MethodInfo _method;

		public GenericSingleton(MethodInfo method, IArray<ParameterExpression> expressions)
		{
			_method    = method;
			Parameters = expressions;
		}

		public IArray<ParameterExpression> Parameters { get; }

		public Expression Get(Type parameter)
		{
			var call = Expression.Call(Expression.Constant(Singletons.Default), _method,
			                           Expression.Constant(parameter));
			var result = Expression.Convert(call, parameter);
			return result;
		}
	}
}