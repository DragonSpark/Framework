using DragonSpark.Application.AspNet.Runtime;
using DragonSpark.Application.Model;
using DragonSpark.Application.Model.Selections;
using DragonSpark.Compose;
using DragonSpark.Contracts.Uploads;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public abstract class Completed<T> : IInput<WorkspacePath, T>
{
	readonly ITemporaryPath         _path;
	readonly IStopAware<string, T?> _entry;

	protected Completed(ITemporaryPath path, IStopAware<string, T?> entry)
	{
		_path  = path;
		_entry = entry;
	}

	public async ValueTask<T> Get(Stop<UserInput<WorkspacePath>> parameter)
	{
		var path  = _path.Get(parameter);
		var entry = await _entry.Off(new(path, parameter));
		return entry.Verify();
	}
}