using System.Buffers;
using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;

namespace DragonSpark.Runtime.Invocation.Expressions;

public sealed class MemberPathExpression : IFormatter<Expression>
{
    public static MemberPathExpression Default { get; } = new();

    MemberPathExpression() : this(Members.Default) {}

    readonly ILease<Expression, MemberInfo> _members;

    public MemberPathExpression(ILease<Expression, MemberInfo> members) => _members = members;

    public string Get(Expression parameter)
    {
        using var members = _members.Get(parameter);
        using var values  = members.AsValueEnumerable().Select(x => x.Name).ToArray(ArrayPool<string>.Shared);
        return string.Join('.', values);
    }
}