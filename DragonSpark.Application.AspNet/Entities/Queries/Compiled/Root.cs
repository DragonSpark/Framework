using System.Linq.Expressions;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Root : ISelect<MemberExpression, Expression?>
{
    public static Root Default { get; } = new();

    Root() { }

    public Expression? Get(MemberExpression parameter)
    {
        var current = parameter;
        while (current.Expression is MemberExpression next)
        {
            current = next;
        }

        return current.Expression;
    }
}
