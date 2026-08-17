using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Server.Requests;

namespace DragonSpark.Azure.Storage.Uploads;

public class Save : IFiles
{
	readonly IAppend     _append;
	readonly IUploadRoot _root;

	protected Save(IContainer container, IUploadRoot root) : this(container.Append(), root) {}

	protected Save(IAppend append, IUploadRoot root)
	{
		_append = append;
		_root   = root;
	}

	public async ValueTask Get(Stop<Input<FileSession>> parameter)
	{
		var ((principal, (workspace, session, file)), stop) = parameter;
		var             input  = workspace.HasValue ? new Input(principal, workspace.Value) : default(Input?);
		var             root   = input.HasValue ? _root.Get(input.Value) : _root.Get(principal);
		var             path   = $"{root}/{session}/{file.FileName}";
		await using var source = file.OpenReadStream();
		await _append.Off(new(new(path, file.ContentType, source), stop));
	}
}