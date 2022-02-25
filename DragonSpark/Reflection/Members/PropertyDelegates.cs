using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public sealed class PropertyDelegates : ISelect<(Type Owner, string Name), Func<object, object>>
{
	public static PropertyDelegates Default { get; } = new();

	PropertyDelegates() : this(PropertyValueDelegates.Default, AllInstanceFlags.Default) {}

	readonly IPropertyValueDelegate _delegates;
	readonly BindingFlags           _flags;

	public PropertyDelegates(IPropertyValueDelegate delegates, BindingFlags flags)
	{
		_delegates = delegates;
		_flags     = flags;
	}

	public Func<object, object> Get((Type Owner, string Name) parameter)
	{
		var (owner, name) = parameter;
		var property = owner.GetProperty(name, _flags)
		                    .Verify($"Could not locate property '{name}' on type '{owner}'");
		var result = _delegates.Get(property);
		return result;
	}
}

public sealed class PropertyDelegates<T> : IPropertyDelegates<T>
{
	public static PropertyDelegates<T> Default { get; } = new();

	PropertyDelegates() : this(PropertyValueDelegates<T>.Default, AllInstanceFlags.Default) {}

	readonly IPropertyValueDelegate<T> _delegates;
	readonly BindingFlags              _flags;

	public PropertyDelegates(IPropertyValueDelegate<T> delegates, BindingFlags flags)
	{
		_delegates = delegates;
		_flags     = flags;
	}

	public Func<object, T> Get((Type Owner, string Name) parameter)
	{
		var (owner, name) = parameter;
		var property = owner.GetProperty(name, _flags)
		                    .Verify($"Could not locate property '{name}' on type '{owner}'");
		var result = _delegates.Get(property);
		return result;
	}
}