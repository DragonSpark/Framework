using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class FieldValueDelegate : IFieldValueDelegate
{
	readonly IGeneric<IFieldValueDelegate> _generic;

	public static FieldValueDelegate Default { get; } = new FieldValueDelegate();

	FieldValueDelegate()
		: this(new Generic<IFieldValueDelegate>(typeof(FieldValueDelegateAdapter<,>))) {}

	public FieldValueDelegate(IGeneric<IFieldValueDelegate> generic) => _generic = generic;

	public Func<object, object> Get(FieldInfo parameter)
		=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.FieldType)
		           .Get(parameter);
}

sealed class FieldValueDelegate<T> : IFieldValueDelegate<T>
{
	readonly IGeneric<IFieldValueDelegate<T>> _generic;

	public static FieldValueDelegate<T> Default { get; } = new FieldValueDelegate<T>();

	FieldValueDelegate()
		: this(new Generic<IFieldValueDelegate<T>>(typeof(FieldDelegateAdapter<,>))) {}

	public FieldValueDelegate(IGeneric<IFieldValueDelegate<T>> generic) => _generic = generic;

	public Func<object, T> Get(FieldInfo parameter)
		=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.FieldType)
		           .Get(parameter);
}
sealed class FieldValueDelegate<T, TValue> : IFieldValueDelegate<T, TValue>
{
	public static FieldValueDelegate<T, TValue> Default { get; } = new FieldValueDelegate<T, TValue>();

	FieldValueDelegate() : this(Expression.Parameter(typeof(T), "parameter")) {}

	readonly ParameterExpression _owner;

	public FieldValueDelegate(ParameterExpression owner) => _owner = owner;

	public Func<T, TValue> Get(FieldInfo parameter)
		=> Expression.Lambda<Func<T, TValue>>(Expression.Field(_owner, parameter), _owner).Compile();
}