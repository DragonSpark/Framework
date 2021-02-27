using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class PropertyAssignmentDelegate : IPropertyAssignmentDelegate
	{
		readonly IGeneric<IPropertyAssignmentDelegate> _generic;

		public static PropertyAssignmentDelegate Default { get; } = new();

		PropertyAssignmentDelegate()
			: this(new Generic<IPropertyAssignmentDelegate>(typeof(GeneralPropertyAssignmentDelegateAdapter<,>))) {}

		public PropertyAssignmentDelegate(IGeneric<IPropertyAssignmentDelegate> generic) => _generic = generic;

		public Action<object, object> Get(PropertyInfo parameter)
			=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.PropertyType)
			           .Get(parameter);
	}

	sealed class PropertyAssignmentDelegate<T> : IPropertyAssignmentDelegate<T>
	{
		readonly IGeneric<IPropertyAssignmentDelegate<T>> _generic;

		public static PropertyAssignmentDelegate<T> Default { get; } = new();

		PropertyAssignmentDelegate()
			: this(new Generic<IPropertyAssignmentDelegate<T>>(typeof(PropertyAssignmentDelegateAdapter<,>))) {}

		public PropertyAssignmentDelegate(IGeneric<IPropertyAssignmentDelegate<T>> generic) => _generic = generic;

		public Action<object, T> Get(PropertyInfo parameter)
			=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.PropertyType)
			           .Get(parameter);
	}

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