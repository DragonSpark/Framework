using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class FieldValueDelegateAdapter<T, TValue> : Select<FieldInfo, Func<object, object>>, IFieldValueDelegate
{
	public static FieldValueDelegateAdapter<T, TValue> Default { get; } = new();

	FieldValueDelegateAdapter()
		: base(Start.An.Instance(FieldDelegateAdapter<T, TValue>.Default)
		            .Then()
		            .Select(x => x.Start().Cast<object>().Get().ToDelegate())) {}
}