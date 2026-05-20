using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Invocation.Expressions;

public sealed class ExpressionMemberName : ISelect<Expression, MemberInfo?>
{
    public static ExpressionMemberName Default { get; } = new();

    ExpressionMemberName() {}

    public MemberInfo? Get(Expression parameter)
    {
        var expression = parameter is UnaryExpression u ? u.Operand : parameter;
        var result     = expression is MemberExpression m ? m.Member : null;
        return result;
    }
}