using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Expressions
{
	class ActivateFromArrayExpression : InvokeArrayFactoryBase<ConstructorInfo>
	{
		public static ActivateFromArrayExpression Default { get; } = new ActivateFromArrayExpression();
		ActivateFromArrayExpression() {}

		protected override Expression Apply( ExpressionBodyParameter<ConstructorInfo> parameter, Expression[] arguments ) => Expression.New( parameter.Input, arguments );
	}
}