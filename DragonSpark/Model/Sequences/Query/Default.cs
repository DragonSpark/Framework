using System.Buffers;

namespace DragonSpark.Model.Sequences.Query;

sealed class Default
{
	public static ArrayPool<int> Numbers { get; } = ArrayPool<int>.Shared;
}