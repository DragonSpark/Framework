using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Server.Output;

sealed class ProcessTags : IStopAware<ProcessTagsInput>
{
	readonly ITags        _tags;
	readonly IOutputKey[] _keys;

	public ProcessTags(ITags tags, params IOutputKey[] keys)
	{
		_tags = tags;
		_keys = keys;
	}

	public async ValueTask Get(Stop<ProcessTagsInput> parameter)
	{
		var ((subject, tags), stop) = parameter;
		foreach (var key in _keys.Open())
		{
			await _tags.Off(new(new(subject, key, tags), stop));
		}
	}
}