using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	public sealed class PropertyDelegates : PropertyDelegates<object>, IPropertyDelegates
	{
		public static PropertyDelegates Default { get; } = new PropertyDelegates();

		PropertyDelegates() {}
	}

	public class PropertyDelegates<T> : ReferenceValueTable<Type, Func<PropertyInfo, Func<object, T>>>,
	                                    IPropertyDelegates<T>
	{
		protected PropertyDelegates() : base(x => new PropertyDelegate<T>(x).Then().Stores().New().Get) {}
	}
}