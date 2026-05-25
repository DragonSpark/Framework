using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Compose;

namespace DragonSpark.Runtime.Invocation.Expressions;

public static class Objects
{
	public static MemberInfo GetMemberInfo(this LambdaExpression @this)
		=> ExpressionMemberName.Default.Get(@this.Body).Verify("Member not found.");
}