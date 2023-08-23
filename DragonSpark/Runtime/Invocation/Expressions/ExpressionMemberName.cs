using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Invocation.Expressions;

public sealed class ExpressionMemberName : ISelect<LambdaExpression, MemberInfo>
{
	public static ExpressionMemberName Default { get; } = new ();

	ExpressionMemberName() {}

	public MemberInfo Get(LambdaExpression parameter)
		=> (parameter.Body.AsTo<UnaryExpression, Expression>(x => x.Operand).Account()
		    ??
		    parameter.Body).To<MemberExpression>()
		                   .Member;
}