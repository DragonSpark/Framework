using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Invocation.Expressions;

public sealed class ExpressionMemberName : ISelect<LambdaExpression, MemberInfo>
{
	public static ExpressionMemberName Default { get; } = new ExpressionMemberName();

	ExpressionMemberName() {}

	public MemberInfo Get(LambdaExpression parameter)
		=> (parameter.Body.AsTo<UnaryExpression, Expression>(unaryExpression => unaryExpression.Operand).Account()
		    ??
		    parameter.Body)
		   .To<MemberExpression>()
		   .Member;
}