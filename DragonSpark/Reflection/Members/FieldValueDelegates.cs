using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public sealed class FieldValueDelegates<T, TValue> : ConcurrentStore<FieldInfo, Func<T, TValue>>,
                                                     IFieldValueDelegate<T, TValue>
{
	public static FieldValueDelegates<T, TValue> Default { get; } = new();

	FieldValueDelegates() : base(FieldValueDelegate<T, TValue>.Default.Then().Stores().New().Get) {}
}