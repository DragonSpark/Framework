using DragonSpark.Compose;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	public sealed class PropertyDelegate : PropertyDelegate<object, object>
	{
		public static PropertyDelegate Default { get; } = new PropertyDelegate();

		PropertyDelegate() {}
	}

	public class PropertyDelegate<T, TProperty> : PropertyDelegate<TProperty>
	{
		public PropertyDelegate() : base(A.Type<T>()) {}
	}

	public class PropertyDelegate<T> : IPropertyDelegate<T>
	{
		readonly ParameterExpression _owner;
		readonly Expression          _converted;
		readonly Type                _targetType;

		public PropertyDelegate(Type instanceType)
			: this(instanceType, Expression.Parameter(typeof(object), "parameter")) {}

		public PropertyDelegate(Type instanceType, ParameterExpression owner)
			: this(owner, Expression.Convert(owner, instanceType), A.Type<T>()) {}

		public PropertyDelegate(ParameterExpression owner, Expression converted, Type targetType)
		{
			_owner           = owner;
			_converted       = converted;
			_targetType = targetType;
		}

		public Func<object, T> Get(PropertyInfo parameter)
		{
			var property = Expression.Convert(Expression.Property(_converted, parameter), _targetType);
			var lambda   = Expression.Lambda<Func<object, T>>(property, _owner);
			var result   = lambda.Compile();
			return result;
		}
	}
}