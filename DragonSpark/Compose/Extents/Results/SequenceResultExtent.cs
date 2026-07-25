using DragonSpark.Model.Sequences;

namespace DragonSpark.Compose.Extents.Results;

public sealed class SequenceResultExtent<T> : ResultExtent<IEnumerable<T>>
{
	public static SequenceResultExtent<T> Default { get; } = new();

	SequenceResultExtent() {}

	public ArrayResultExtent<T> Array => ArrayResultExtent<T>.Default;

	public ResultExtent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
}