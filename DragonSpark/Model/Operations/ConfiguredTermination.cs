using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class ConfiguredTermination<T> : IOperation
{
	readonly IResult<ValueTask<T>> _result;
	readonly Func<T, ValueTask>    _configure;

	public ConfiguredTermination(IResult<ValueTask<T>> result, Func<T, ValueTask> configure)
	{
		_result    = result;
		_configure = configure;
	}

	public async ValueTask Get()
	{
		var parameter = await _result.Get();
		await _configure(parameter);
	}
}