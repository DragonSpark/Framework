using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading.Tasks;

namespace DragonSpark.Server.Output;

public sealed class ClearUserOutputs<T> : IStopAware<T>
{
	readonly IStopAware<T, uint> _user;
	readonly IOutputCacheStore   _output;
	readonly IUserOutputKey      _key;

	public ClearUserOutputs(IStopAware<T, uint> user, IOutputCacheStore output, IUserOutputKey key)
	{
		_user   = user;
		_output = output;
		_key    = key;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;
		var author = await _user.Off(parameter);
		var tag    = _key.Get(new UserInput<T>(author, subject));
		await _output.EvictByTagAsync(tag, stop).Off();
	}
}