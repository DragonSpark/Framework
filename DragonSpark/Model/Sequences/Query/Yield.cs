using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query;

public sealed class Yield<T> : Select<T, T[]>
{
	public static Yield<T> Default { get; } = new Yield<T>();

	Yield() : base(x => new[] {x}) {}
}