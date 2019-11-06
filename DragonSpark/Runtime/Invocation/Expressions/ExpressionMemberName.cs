using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	public sealed class ExpressionMemberName : ISelect<LambdaExpression, MemberInfo>
	{
		public static ExpressionMemberName Default { get; } = new ExpressionMemberName();

		ExpressionMemberName() {}

		public MemberInfo Get(LambdaExpression parameter)
			=> (parameter.Body.AsTo<UnaryExpression, Expression>(unaryExpression => unaryExpression.Operand) ??
			    parameter.Body)
			   .To<MemberExpression>()
			   .Member;
	}
}