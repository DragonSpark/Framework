using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	sealed class IsDecoratedWith<T> : Condition<ICustomAttributeProvider> where T : Attribute
	{
		public static IsDecoratedWith<T> Default { get; } = new IsDecoratedWith<T>();

		IsDecoratedWith() : base(x => x.Has<T>()) {}
	}
}