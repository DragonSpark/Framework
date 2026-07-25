using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Sequences.Collections;

public sealed class HasNone<T> : InverseCondition<ICollection<T>>
{
	public static HasNone<T> Default { get; } = new();

	HasNone() : base(HasAny<T>.Default) {}
}