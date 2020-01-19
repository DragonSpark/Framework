using DragonSpark.Model.Selection.Conditions;
using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections
{
	public sealed class HasAny : Condition<ICollection>
	{
		public static HasAny Default { get; } = new HasAny();

		HasAny() : base(x => x.Count > 0) {}
	}

	public sealed class HasAny<T> : Condition<ICollection<T>>
	{
		public static HasAny<T> Default { get; } = new HasAny<T>();

		HasAny() : base(x => x.Count > 0) {}
	}
}