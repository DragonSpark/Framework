using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Runtime.Invocation.Expressions;

public sealed class Members : ILease<Expression, MemberInfo>
{
    public static Members Default { get; } = new();

    Members() {}

    [MustDisposeResource]
    public Leasing<MemberInfo> Get(Expression parameter)
    {
        using var builder = ArrayBuilder.New<MemberInfo>(32);
        while (parameter is MemberExpression m)
        {
            builder.Add(m.Member);
            parameter = m.Expression is UnaryExpression u ? u.Operand : m.Expression!;
        }

        builder.AsSpan().Reverse();
        return builder.AsLease();
    }
}