using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class UploadRequestParser : ISelect<IFormCollection, UploadRequest>
{
	public static UploadRequestParser Default { get; } = new();

	UploadRequestParser() : this(CurrentChunk.Default, WorkspaceValue.Default, SessionValue.Default) {}

	readonly ISelect<IFormCollection, CurrentChunkView?> _chunk;
	readonly ISelect<FormChunkValueInput, Guid?>         _workspace, _session;

	public UploadRequestParser(ISelect<IFormCollection, CurrentChunkView?> chunk,
	                           ISelect<FormChunkValueInput, Guid?> workspace,
	                           ISelect<FormChunkValueInput, Guid?> session)
	{
		_chunk     = chunk;
		_workspace = workspace;
		_session   = session;
	}

	public UploadRequest Get(IFormCollection parameter)
	{
		var chunk = _chunk.Get(parameter);
		var input = new FormChunkValueInput(parameter, chunk?.Index);
		return new(_workspace.Get(input), _session.Get(input).Value(),
		           chunk is null || chunk.Value.Index == chunk.Value.Total - 1);
	}
}