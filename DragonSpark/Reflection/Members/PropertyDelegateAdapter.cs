using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class PropertyDelegateAdapter<T, TValue> : Select<PropertyInfo, Func<object, TValue>>,
	                                                  IPropertyValueDelegate<TValue>
	{
		public static PropertyDelegateAdapter<T, TValue> Default { get; } = new PropertyDelegateAdapter<T, TValue>();

		PropertyDelegateAdapter() : base(Start.An.Instance(PropertyValueDelegates<T, TValue>.Default)
		                                      .Select(Start.A.Selection.Of.Any.By.CastDown<T>().Get().Select)
		                                      .Then()
		                                      .Delegate()) {}
	}
}