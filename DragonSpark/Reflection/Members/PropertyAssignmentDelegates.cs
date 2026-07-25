using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public sealed class PropertyAssignmentDelegates : ConcurrentStore<PropertyInfo, Action<object, object?>>,
                                                  IPropertyAssignmentDelegate
{
	public static PropertyAssignmentDelegates Default { get; } = new ();

	PropertyAssignmentDelegates() : base(PropertyAssignmentDelegate.Default.Then().Stores().New().Get) {}
}

public sealed class PropertyAssignmentDelegates<T, TValue> : ConcurrentStore<PropertyInfo, Action<T, TValue>>,
                                                             IPropertyAssignmentDelegate<T, TValue>
{
	public static PropertyAssignmentDelegates<T, TValue> Default { get; } = new();

	PropertyAssignmentDelegates() : base(PropertyAssignmentDelegate<T, TValue>.Default.Then().Stores().New().Get) {}
}