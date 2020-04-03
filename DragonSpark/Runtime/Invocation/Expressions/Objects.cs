using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	public static class Objects
	{
		public static MemberInfo GetMemberInfo(this LambdaExpression expression)
			=> ExpressionMemberName.Default.Get(expression);
	}
}