using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class PropertyValueDelegate : IPropertyValueDelegate
{
	readonly IGeneric<IPropertyValueDelegate> _generic;

	public static PropertyValueDelegate Default { get; } = new();

	PropertyValueDelegate()
		: this(new Generic<IPropertyValueDelegate>(typeof(GeneralPropertyDelegateAdapter<,>))) {}

	public PropertyValueDelegate(IGeneric<IPropertyValueDelegate> generic) => _generic = generic;

	public Func<object, object> Get(PropertyInfo parameter)
		=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.PropertyType)
		           .Get(parameter);
}

sealed class PropertyValueDelegate<T> : IPropertyValueDelegate<T>
{
	readonly IGeneric<IPropertyValueDelegate<T>> _generic;

	public static PropertyValueDelegate<T> Default { get; } = new();

	PropertyValueDelegate()
		: this(new Generic<IPropertyValueDelegate<T>>(typeof(PropertyDelegateAdapter<,>))) {}

	public PropertyValueDelegate(IGeneric<IPropertyValueDelegate<T>> generic) => _generic = generic;

	public Func<object, T> Get(PropertyInfo parameter)
		=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.PropertyType)
		           .Get(parameter);
}
sealed class PropertyValueDelegate<T, TValue> : IPropertyValueDelegate<T, TValue>
{
	public static PropertyValueDelegate<T, TValue> Default { get; } = new();

	PropertyValueDelegate() : this(Expression.Parameter(typeof(T), "parameter")) {}

	readonly ParameterExpression _owner;

	public PropertyValueDelegate(ParameterExpression owner) => _owner = owner;

	public Func<T, TValue> Get(PropertyInfo parameter)
		=> Expression.Lambda<Func<T, TValue>>(Expression.Property(_owner, parameter), _owner).Compile();
}