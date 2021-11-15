using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Types;

sealed class ActivateExpressions : IActivateExpressions
{
	readonly Array<Type> _types;

	public ActivateExpressions(Array<Type> parameters)
		: this(parameters,
		       new Instances<ParameterExpression>(parameters.Open()
		                                                    .Select(Defaults.Parameter)
		                                                    .ToArray())) {}

	public ActivateExpressions(Array<Type> types, IArray<ParameterExpression> parameters)
	{
		Parameters = parameters;
		_types     = types;
	}

	public Expression Get(Type parameter)
	{
		var constructors = parameter.GetTypeInfo().DeclaredConstructors.ToArray();
		var constructor = constructors.Only() ??
		                  parameter.GetConstructors().Only() ??
		                  parameter.GetConstructor(_types) ??
		                  constructors.OrderBy(x => x.GetParameters().Length)
		                              .FirstOrDefault(x => x.Has<ActivatorUtilitiesConstructorAttribute>())
		                  ??
		                  throw new InvalidOperationException($"Constructor for type '{parameter}' not found!");
		var types      = constructor.GetParameters().Select(x => x.ParameterType);
		var parameters = Parameters.Get().Open().Zip(types, Defaults.ExpressionZip);
		var result     = Expression.New(constructor, parameters);
		return result;
	}

	public IArray<ParameterExpression> Parameters { get; }
}