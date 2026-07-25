using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public sealed class PropertyValueDelegates : ConcurrentStore<PropertyInfo, Func<object, object>>,
                                             IPropertyValueDelegate
{
	public static PropertyValueDelegates Default { get; } = new();

	PropertyValueDelegates() : base(PropertyValueDelegate.Default.Then().Stores().New().Get) {}
}

public sealed class PropertyValueDelegates<T> : ConcurrentStore<PropertyInfo, Func<object, T>>,
                                                IPropertyValueDelegate<T>
{
	public static PropertyValueDelegates<T> Default { get; } = new();

	PropertyValueDelegates() : base(PropertyValueDelegate<T>.Default.Then().Stores().New().Get) {}
}

public sealed class PropertyValueDelegates<T, TValue> : ConcurrentStore<PropertyInfo, Func<T, TValue>>,
														IPropertyValueDelegate<T, TValue>
{
	public static PropertyValueDelegates<T, TValue> Default { get; } = new();

	PropertyValueDelegates() : base(PropertyValueDelegate<T, TValue>.Default.Then().Stores().New().Get) {}
}