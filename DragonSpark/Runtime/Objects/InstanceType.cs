using System;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Objects
{
	public sealed class InstanceType<T> : Select<T, Type>
	{
		public static InstanceType<T> Default { get; } = new InstanceType<T>();

		InstanceType() : base(x => x?.GetType() ?? Type<T>.Instance) {}
	}
}