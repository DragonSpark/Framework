using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	public interface IAttributes<T> : IConditional<ICustomAttributeProvider, Array<T>> where T : Attribute {}

	public sealed class IsDecoratedWith<T> : Condition<ICustomAttributeProvider> where T : Attribute
	{
		public static IsDecoratedWith<T> Default { get; } = new IsDecoratedWith<T>();

		IsDecoratedWith() : base(x => x.Has<T>()) {}
	}
}