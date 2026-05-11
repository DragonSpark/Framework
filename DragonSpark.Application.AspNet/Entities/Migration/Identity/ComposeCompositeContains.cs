using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ComposeCompositeContains<T> : ISelect<ComposeContainsInput, Expression<Func<T, bool>>>
{
	public static ComposeCompositeContains<T> Default { get; } = new();

	readonly ParameterExpression _entity;

	ComposeCompositeContains() : this(Expression.Parameter(typeof(T), "x")) {}

	public ComposeCompositeContains(ParameterExpression entity) => _entity = entity;

	public Expression<Func<T, bool>> Get(ComposeContainsInput parameter)
	{
		var (metadata, input) = parameter;

		var         properties = metadata.FindPrimaryKey().Verify().Properties;
		Expression? body       = null;

		foreach (var row in input)
		{
			if (row is object[] values && values.Length == properties.Count)
			{
				Expression? and = null;

				for (var i = 0; i < properties.Count; i++)
				{
					var left     = Expression.Property(_entity, properties[i].PropertyInfo.Verify());
					var type     = properties[i].ClrType;
					var constant = Expression.Constant(Convert.ChangeType(values[i], type), type);
					var equal    = Expression.Equal(left, constant);
					and = and is null ? equal : Expression.AndAlso(and, equal);
				}

				body = body is null ? and : Expression.OrElse(body, and!);
			}
		}

		body ??= Expression.Constant(false);

		return Expression.Lambda<Func<T, bool>>(body, _entity);
	}
}