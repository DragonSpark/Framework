using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class FieldDelegateAdapter<T, TValue> : Select<FieldInfo, Func<object, TValue>>, IFieldValueDelegate<TValue>
{
	public static FieldDelegateAdapter<T, TValue> Default { get; } = new();

	FieldDelegateAdapter() : base(Start.An.Instance(FieldValueDelegates<T, TValue>.Default)
	                                   .Select(Start.A.Selection.Of.Any.By.CastDown<T>().Get().Select)
	                                   .Then()
	                                   .Delegate()) {}
}