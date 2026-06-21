using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query;

public sealed class Allocate<T> : Select<T, T[]>
{
	public static Allocate<T> Default { get; } = new();

	Allocate() : base(x => [x]) {}
}