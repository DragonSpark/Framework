using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	public sealed class PropertyAssignmentDelegates<T, TValue> : ReferenceValueTable<PropertyInfo, Action<T, TValue>>,
	                                                             IPropertyAssignmentDelegate<T, TValue>
	{
		public static PropertyAssignmentDelegates<T, TValue> Default { get; }
			= new PropertyAssignmentDelegates<T, TValue>();

		PropertyAssignmentDelegates() : base(PropertyAssignmentDelegate<T, TValue>.Default.Then().Stores().New().Get) {}
	}
}