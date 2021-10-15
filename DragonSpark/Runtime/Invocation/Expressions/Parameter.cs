using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Runtime.Invocation.Expressions;

sealed class Parameter<T> : FixedSelectedSingleton<string, ParameterExpression>
{
	public static Parameter<T> Default { get; } = new Parameter<T>();

	public Parameter(string name = "parameter") : base(Parameters<T>.Default, name) {}
}

sealed class Parameter : Invocation1<Type, string, ParameterExpression>
{
	public static Parameter Default { get; } = new Parameter();

	Parameter() : this(A.Type<object[]>()) {}

	public Parameter(Type parameter) : base(Expression.Parameter, parameter) {}
}