using DragonSpark.Model.Selection.Conditions;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections
{
	public sealed class HasNone<T> : InverseCondition<ICollection<T>>
	{
		public static HasNone<T> Default { get; } = new HasNone<T>();

		HasNone() : base(HasAny<T>.Default) {}
	}
}