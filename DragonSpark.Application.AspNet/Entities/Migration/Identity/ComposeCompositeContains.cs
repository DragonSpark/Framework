using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ComposeCompositeContains<T> : ISelect<ComposeContainsInput, Expression<Func<T, bool>>>
{
	public static ComposeCompositeContains<T> Default { get; } = new();

	readonly ParameterExpression       _entity;
	readonly ISelect<Type, MethodInfo> _method;

	ComposeCompositeContains() : this(Expression.Parameter(typeof(T), "x"), PropertyMethods.Default) {}

	public ComposeCompositeContains(ParameterExpression entity, ISelect<Type, MethodInfo> method)
	{
		_entity = entity;
		_method = method;
	}

	public Expression<Func<T, bool>> Get(ComposeContainsInput parameter)
	{
		var (metadata, input) = parameter;

		var         properties = metadata.FindPrimaryKey().Verify().Properties;
		Expression? body       = null;

		using var builder = ArrayBuilder.New<Expression>(properties.Count);
		for (var i = 0; i < properties.Count; i++)
		{
			var property = properties[i];
			builder.UncheckedAdd(property.PropertyInfo is {} p
				                     ? Expression.Property(_entity, p)
				                     : Expression.Call(null, _method.Get(property.ClrType), _entity,
				                                       Expression.Constant(property.Name)));
		}

		var expressions = builder.AsSpan();

		foreach (var row in input)
		{
			if (row is object[] values && values.Length == properties.Count)
			{
				Expression? and = null;

				for (var i = 0; i < properties.Count; i++)
				{
					var type     = properties[i].ClrType;
					var constant = Expression.Constant(Convert.ChangeType(values[i], type), type);
					var equal    = Expression.Equal(expressions[i], constant);
					and = and is not null ? Expression.AndAlso(and, equal) : equal;
				}

				body = body is not null ? Expression.OrElse(body, and!) : and;
			}
		}

		body ??= Expression.Constant(false);

		return Expression.Lambda<Func<T, bool>>(body, _entity);
	}
}