using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	sealed class ConstructorParameters<T> : ConstructorParameters
	{
		public static ConstructorParameters<T> Default { get; } = new ConstructorParameters<T>();

		ConstructorParameters() : base(Parameter<T>.Default.Get()) {}
	}

	class ConstructorParameters : ISelect<ConstructorInfo, IEnumerable<Expression>>
	{
		readonly ParameterExpression _parameter;

		public ConstructorParameters(ParameterExpression parameter) => _parameter = parameter;

		public IEnumerable<Expression> Get(ConstructorInfo parameter)
			=> ExtensionMethods.Prepend<Expression>(parameter.GetParameters()
			                                                 .Skip(1)
			                                                 .Select(x => Expression.Constant(x.DefaultValue,
			                                                                                  x.ParameterType)),
			                                        _parameter);
	}
}