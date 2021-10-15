using DragonSpark.Model.Selection;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Root : ISelect<MemberExpression, Expression?>
{
	public static Root Default { get; } = new Root();

	Root() {}

	public Expression? Get(MemberExpression parameter)
	{
		var current = parameter;
		while (current.Expression is MemberExpression next)
		{
			current = next;
		}

		return current?.Expression;
	}
}