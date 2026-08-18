using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class CurrentChunk : ISelect<IFormCollection, CurrentChunkView?>
{
	public static CurrentChunk Default { get; } = new();

	CurrentChunk() : this(Chunk.Default, Chunks.Default) {}

	readonly ISelect<IFormCollection, ushort?> _index;
	readonly ISelect<IFormCollection, ushort?> _total;

	public CurrentChunk(ISelect<IFormCollection, ushort?> index, ISelect<IFormCollection, ushort?> total)
	{
		_index = index;
		_total = total;
	}

	public CurrentChunkView? Get(IFormCollection parameter)
	{
		var total = _total.Get(parameter);
		return total.HasValue ? new(_index.Get(parameter).Value(), total.Value) : null;
	}
}