using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public sealed class PropertyAssignmentDelegates : ReferenceValueTable<PropertyInfo, Action<object, object>>,
                                                  IPropertyAssignmentDelegate
{
	public static PropertyAssignmentDelegates Default { get; } = new ();

	PropertyAssignmentDelegates() : base(PropertyAssignmentDelegate.Default.Then().Stores().New().Get) {}
}


public sealed class PropertyAssignmentDelegates<T> : ReferenceValueTable<PropertyInfo, Action<object, T>>,
                                                     IPropertyAssignmentDelegate<T>
{
	public static PropertyAssignmentDelegates<T> Default { get; } = new ();

	PropertyAssignmentDelegates() : base(PropertyAssignmentDelegate<T>.Default.Then().Stores().New().Get) {}
}

public sealed class PropertyAssignmentDelegates<T, TValue> : ReferenceValueTable<PropertyInfo, Action<T, TValue>>,
                                                             IPropertyAssignmentDelegate<T, TValue>
{
	public static PropertyAssignmentDelegates<T, TValue> Default { get; }
		= new PropertyAssignmentDelegates<T, TValue>();

	PropertyAssignmentDelegates() : base(PropertyAssignmentDelegate<T, TValue>.Default.Then().Stores().New().Get) {}
}