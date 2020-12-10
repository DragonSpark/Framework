using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class PropertyAssignmentDelegate<T, TValue> : IPropertyAssignmentDelegate<T, TValue>
	{
		public static PropertyAssignmentDelegate<T, TValue> Default { get; }
			= new PropertyAssignmentDelegate<T, TValue>();

		PropertyAssignmentDelegate() : this(Expression.Parameter(typeof(T), "owner"),
		                                    Expression.Parameter(typeof(TValue), "parameter")) {}

		readonly ParameterExpression _owner, _value;

		public PropertyAssignmentDelegate(ParameterExpression owner, ParameterExpression value)
		{
			_owner = owner;
			_value = value;
		}

		public Action<T, TValue> Get(PropertyInfo parameter)
		{
			var assign = Expression.Assign(Expression.Property(_owner, parameter), _value);
			var lambda = Expression.Lambda<Action<T, TValue>>(assign, _owner, _value);
			var result = lambda.Compile();
			return result;
		}
	}
}