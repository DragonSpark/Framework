using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Reflection.Types
{
	sealed class ActivateExpressions : IActivateExpressions
	{
		readonly Array<Type> _types;

		public ActivateExpressions(Array<Type> parameters)
			: this(parameters,
			       new ArrayInstance<ParameterExpression>(parameters.Open()
			                                                        .Select(Defaults.Parameter)
			                                                        .ToArray())) {}

		public ActivateExpressions(Array<Type> types, IArray<ParameterExpression> parameters)
		{
			Parameters = parameters;
			_types     = types;
		}

		public Expression Get(Type parameter)
		{
			var constructor = parameter.GetTypeInfo().DeclaredConstructors.Only() ??
			                  parameter.GetConstructors().Only() ??
			                  parameter.GetConstructor(_types);
			var types      = constructor.GetParameters().Select(x => x.ParameterType);
			var parameters = Parameters.Get().Open().Zip(types, Defaults.ExpressionZip);
			var result     = Expression.New(constructor, parameters);
			return result;
		}

		public IArray<ParameterExpression> Parameters { get; }
	}
}