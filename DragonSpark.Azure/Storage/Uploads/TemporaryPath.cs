using DragonSpark.Application.AspNet.Runtime;
using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Contracts.Uploads;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class TemporaryPath : ITemporaryPath
{
	readonly TemporaryUserPath _root;

	public TemporaryPath(TemporaryUserPath root) => _root = root;

	public string Get(UserInput<WorkspacePath> parameter)
	{
		var (user, (workspace, referenced)) = parameter;
		var root = _root.Get(new(user, workspace.ToString()));
		return !referenced.IsNullOrEmpty() ? $"{root}/{referenced}" : root;
	}
}