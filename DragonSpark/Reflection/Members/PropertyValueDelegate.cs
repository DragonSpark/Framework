using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	/*public sealed class PropertyValueDelegate<T> : IPropertyValueDelegate<object, T>
	{
		public static PropertyValueDelegate<T> Default { get; } = new PropertyValueDelegate<T>();

		PropertyValueDelegate()
			: this(new Generic<IPropertyValueDelegate<object, T>>(typeof(PropertyDelegateAdapter<,>)), A.Type<T>()) {}

		readonly IGeneric<IPropertyValueDelegate<object, T>> _generic;
		readonly Type                                        _ownerType;

		public PropertyValueDelegate(IGeneric<IPropertyValueDelegate<object, T>> generic, Type ownerType)
		{
			_generic   = generic;
			_ownerType = ownerType;
		}

		public Func<object, T> Get(PropertyInfo parameter)
			=> _generic.Get(_ownerType, parameter.PropertyType).Get(parameter);
	}*/


	sealed class PropertyValueDelegate<T, TValue> : IPropertyValueDelegate<T, TValue>
	{
		public static PropertyValueDelegate<T, TValue> Default { get; } = new PropertyValueDelegate<T, TValue>();

		PropertyValueDelegate() : this(Expression.Parameter(typeof(T), "parameter")) {}

		readonly ParameterExpression _owner;

		public PropertyValueDelegate(ParameterExpression owner) => _owner = owner;

		public Func<T, TValue> Get(PropertyInfo parameter)
			=> Expression.Lambda<Func<T, TValue>>(Expression.Property(_owner, parameter), _owner).Compile();
	}
}