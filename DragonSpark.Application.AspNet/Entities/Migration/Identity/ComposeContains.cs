using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Collections;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ComposeContains<T> : ISelect<ComposeContainsInput, Expression<Func<T, bool>>>
{
	public static ComposeContains<T> Default { get; } = new();

	ComposeContains() : this(ComposeKeySelector.Default, Cast.Default, Expression.Parameter(typeof(T), "y")) {}

	readonly ISelect<IEntityType, LambdaExpression> _key;
	readonly ISelect<CastInput, Array>              _cast;
	readonly ParameterExpression                    _y;

	public ComposeContains(ISelect<IEntityType, LambdaExpression> key, ISelect<CastInput, Array> cast,
	                       ParameterExpression y)
	{
		_key  = key;
		_cast = cast;
		_y    = y;
	}

	public Expression<Func<T, bool>> Get(ComposeContainsInput parameter)
	{
		var (metadata, keys) = parameter;
		var key     = _key.Get(metadata);
		var x       = key.Parameters[0];
		var body    = new ReplaceParameterVisitor(x, _y).Visit(key.Body);
		var objects = _cast.Get(new(keys, key.ReturnType));
		var contains = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), [key.ReturnType],
		                               Expression.Constant(objects), body);

		return Expression.Lambda<Func<T, bool>>(contains, _y);
	}

	sealed class ReplaceParameterVisitor : ExpressionVisitor
	{
		readonly ParameterExpression _from;
		readonly Expression          _to;

		public ReplaceParameterVisitor(ParameterExpression from, Expression to)
		{
			_from = from;
			_to   = to;
		}

		protected override Expression VisitParameter(ParameterExpression node) => node == _from ? _to : node;
	}
}