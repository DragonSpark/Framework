namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

class Class2;

/*public static class Extensions
{
	[Expandable(nameof(OrderedImplementation))]
	public static IOrderedQueryable<T> Ordered<T>(this IQueryable<T> @this, [UsedImplicitly] string parameter)
		=> OrderedImplementation<T>(parameter).Compile().Invoke(@this);

	static Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> OrderedImplementation<T>(string parameter)
		=> Evaluation.Ordered<T>.Default.Get(parameter);
}

sealed class Ordered<T> : ISelect<string, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>>
{
	public static Ordered<T> Default { get; } = new();

	Ordered() : this(Expression.Parameter(A.Type<IQueryable<T>>(), "x"), ExpressionBody<T>.Default) {}

	readonly ParameterExpression _start;
	readonly IExpressionBody     _body;

	public Ordered(ParameterExpression start, IExpressionBody body)
	{
		_start = start;
		_body  = body;
	}

	public Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> Get(string parameter)
	{
		var steps = parameter.Split(',', StringSplitOptions.TrimEntries);
		var body  = (Expression)_start;
		for (byte i = 0; i < steps.Length; i++)
		{
			var parts      = steps[i].Split(' ', StringSplitOptions.TrimEntries);
			var descending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase);
			var name       = $"{(i == 0 ? "Order" : "Then")}By{(descending ? "Descending" : string.Empty)}";
			body = _body.Get(new(body, name, parts[0]));
		}

		var lambda = Expression.Lambda(body, _start);
		var result = (Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>)lambda;
		return result;
	}
}

public readonly record struct OrderByExpressionInput(Expression Body, string Method, string Property);

public interface IExpressionBody : ISelect<OrderByExpressionInput, MethodCallExpression>;

sealed class ExpressionBody<T> : IExpressionBody
{
	public static ExpressionBody<T> Default { get; } = new();

	ExpressionBody() : this(typeof(Queryable), A.Type<T>()) {}

	readonly Type _queryable, _entity;

	public ExpressionBody(Type queryable, Type entity)
	{
		_queryable = queryable;
		_entity    = entity;
	}

	public MethodCallExpression Get(OrderByExpressionInput parameter)
	{
		var (source, method, property) = parameter;

		var member = _entity.GetProperty(property).Verify();
		var p      = Expression.Parameter(_entity, "p");
		var access = Expression.MakeMemberAccess(p, member);
		var lambda = Expression.Lambda(access, p);
		var result = Expression.Call(_queryable, method, [_entity, member.PropertyType], source,
		                             Expression.Quote(lambda));
		return result;
	}
}*/