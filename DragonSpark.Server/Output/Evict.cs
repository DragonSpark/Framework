using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output;

public sealed class Evict<T> : IStopAware<T> where T : notnull
{
	readonly IOutputCacheStore            _store;
	readonly IStopAware<ProcessTagsInput> _process;

	public Evict(IOutputCacheStore store, params IOutputKey[] keys) : this(store, Tags.Default, keys) {}

	public Evict(IOutputCacheStore store, ITags tags, params IOutputKey[] keys)
		: this(store, new ProcessTags(tags, keys)) {}

	public Evict(IOutputCacheStore store, IStopAware<ProcessTagsInput> process)
	{
		_store   = store;
		_process = process;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;

		var tags = new List<string>();

		await _process.Off(new(new(subject, tags), stop));

		foreach (var tag in tags)
		{
			await _store.EvictByTagAsync(tag, stop).Off();
		}
	}
}

sealed class Evict<TIn, T> : IStopAware<TIn, T> where TIn : notnull
{
	readonly ISelecting<Stop<TIn>, T> _previous;
	readonly IStopAware<TIn>          _evict;

	public Evict(IStopAware<TIn, T> previous, IOutputCacheStore output, params IOutputKey[] keys)
		: this(previous, new Evict<TIn>(output, keys)) {}

	public Evict(IStopAware<TIn, T> previous, IStopAware<TIn> evict)
	{
		_previous = previous;
		_evict    = evict;
	}

	public async ValueTask<T> Get(Stop<TIn> parameter)
	{
		var result = await _previous.Off(parameter);
		await _evict.Off(parameter);
		return result;
	}
}