using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{

	public sealed class FieldValueDelegates : ReferenceValueStore<FieldInfo, Func<object, object>>, IFieldValueDelegate
	{
		public static FieldValueDelegates Default { get; } = new();

		FieldValueDelegates() : base(FieldValueDelegate.Default.Then().Stores().New().Get) {}
	}



	public sealed class FieldValueDelegates<T> : ReferenceValueStore<FieldInfo, Func<object, T>>, IFieldValueDelegate<T>
	{
		public static FieldValueDelegates<T> Default { get; } = new ();

		FieldValueDelegates() : base(FieldValueDelegate<T>.Default.Then().Stores().New().Get) {}
	}


	public sealed class FieldValueDelegates<T, TValue> : ReferenceValueTable<FieldInfo, Func<T, TValue>>,
	                                                     IFieldValueDelegate<T, TValue>
	{
		public static FieldValueDelegates<T, TValue> Default { get; } = new();

		FieldValueDelegates() : base(FieldValueDelegate<T, TValue>.Default.Then().Stores().New().Get) {}
	}
}