using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Server.Requests;

namespace DragonSpark.Azure.Storage.Uploads;

public class Remove : IFiles
{
	readonly IDelete     _delete;
	readonly IUploadRoot _root;

	protected Remove(IContainer container, IUploadRoot root) : this(container.Delete(), root) {}

	protected Remove(IDelete delete, IUploadRoot root)
	{
		_delete = delete;
		_root   = root;
	}

	public async ValueTask Get(Stop<Input<FileSession>> parameter)
	{
		var ((principal, (workspace, session, file)), stop) = parameter;
		var root = workspace.HasValue ? _root.Get(new Input(principal, workspace.Value)) : _root.Get(principal);
		var path = $"{root}/{session}/{file.FileName}";
		await _delete.Off(new(path, stop));
	}
}