using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DragonSpark.Application.Environment.Development;

sealed class QueryTrackingBehaviorVisitor : ExpressionVisitor
{
	public QueryTrackingBehavior? Behavior { get; private set; }

	protected override Expression VisitMethodCall(MethodCallExpression node)
	{
		var name = node.Method.Name;

		switch (name)
		{
			case nameof(EntityFrameworkQueryableExtensions.AsNoTracking):
				Behavior = QueryTrackingBehavior.NoTracking;
				return node;
			case nameof(EntityFrameworkQueryableExtensions.AsNoTrackingWithIdentityResolution):
				Behavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
				return node;
			case nameof(EntityFrameworkQueryableExtensions.AsTracking):
				Behavior = QueryTrackingBehavior.TrackAll;
				return node;
			default:
				return base.VisitMethodCall(node);
		}
	}
}