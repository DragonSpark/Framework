using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DragonSpark.Application.Environment.Development;

public static class QueryExpressionExtensions
{
	public static QueryTrackingBehavior? GetQueryTrackingBehavior(this Expression expression)
	{
		var visitor = new QueryTrackingBehaviorVisitor();
		visitor.Visit(expression);
		return visitor.Behavior.HasValue ? visitor.Behavior.Value : null;
	}
}