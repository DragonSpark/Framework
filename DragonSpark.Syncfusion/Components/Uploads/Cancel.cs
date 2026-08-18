using DragonSpark.Application.AspNet.Runtime;
using DragonSpark.Application.Model;
using DragonSpark.Application.Model.Operations;
using DragonSpark.Compose;
using DragonSpark.Contracts.Uploads;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public abstract class Cancel : IInput<WorkspacePath>
{
	readonly ITemporaryPath _path;
	readonly IStopAware<string>      _clear;

	protected Cancel(ITemporaryPath path, IStopAware<string> clear)
	{
		_path  = path;
		_clear = clear;
	}

	public async ValueTask Get(Stop<UserInput<WorkspacePath>> parameter)
	{
		var path = _path.Get(parameter);
		await _clear.Off(new(path, parameter));
	}
}