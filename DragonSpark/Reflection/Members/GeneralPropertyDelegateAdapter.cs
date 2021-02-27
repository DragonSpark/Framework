using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class GeneralPropertyDelegateAdapter<T, TValue> : Select<PropertyInfo, Func<object, object>>,
	                                                         IPropertyValueDelegate
	{
		public static GeneralPropertyDelegateAdapter<T, TValue> Default { get; } = new();

		GeneralPropertyDelegateAdapter()
			: base(Start.An.Instance(PropertyDelegateAdapter<T, TValue>.Default)
			            .Then()
			            .Select(x => x.Start().Cast<object>().Get().ToDelegate())) {}
	}
}