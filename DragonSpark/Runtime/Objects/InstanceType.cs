using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Objects
{
	public sealed class InstanceType<T> : Select<T, Type>
	{
		public static InstanceType<T> Default { get; } = new InstanceType<T>();

		InstanceType() : base(x => x?.GetType() ?? A.Type<T>()) {}
	}
}