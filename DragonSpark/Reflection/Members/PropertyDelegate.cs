using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	public sealed class PropertyDelegate<T, TProperty> : ISelect<PropertyInfo, Func<object, TProperty>>
	{
		public static PropertyDelegate<T, TProperty> Default { get; } = new PropertyDelegate<T, TProperty>();

		PropertyDelegate() : this(Expression.Parameter(typeof(object), "parameter")) {}

		readonly ParameterExpression _owner;
		readonly UnaryExpression     _converted;

		public PropertyDelegate(ParameterExpression owner) : this(owner, Expression.Convert(owner, A.Type<T>())) {}

		public PropertyDelegate(ParameterExpression owner, UnaryExpression converted)
		{
			_owner     = owner;
			_converted = converted;
		}

		public Func<object, TProperty> Get(PropertyInfo parameter)
		{
			var property = Expression.Property(_converted, parameter);
			var lambda   = Expression.Lambda<Func<object, TProperty>>(property, _owner);
			var result   = lambda.Compile();
			return result;
		}
	}
}