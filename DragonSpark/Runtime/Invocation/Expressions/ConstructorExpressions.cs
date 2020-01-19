using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	sealed class ConstructorExpressions<T> : Select<ConstructorInfo, Expression>
	{
		public static ConstructorExpressions<T> Default { get; } = new ConstructorExpressions<T>();

		ConstructorExpressions() : base(new ConstructorExpressions(ConstructorParameters<T>.Default)) {}
	}

	sealed class ConstructorExpressions : ISelect<ConstructorInfo, Expression>
	{
		public static ConstructorExpressions Default { get; } = new ConstructorExpressions();

		ConstructorExpressions() : this(Selector.Instance) {}

		readonly ISelect<ConstructorInfo, IEnumerable<Expression>> _parameters;

		public ConstructorExpressions(ISelect<ConstructorInfo, IEnumerable<Expression>> parameters)
			=> _parameters = parameters;

		public Expression Get(ConstructorInfo parameter) => new New(_parameters.Get(parameter)).Get(parameter);

		sealed class Selector : Select<ConstructorInfo, IEnumerable<Expression>>
		{
			public static Selector Instance { get; } = new Selector();

			Selector() : base(x => x.GetParameters().Select(y => Expression.Default(y.ParameterType))) {}
		}
	}
}