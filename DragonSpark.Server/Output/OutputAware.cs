using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.OutputCaching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Server.Output;

public class OutputAware<T> : IOperation<T> where T : IUserIdentity
{
	readonly IOperation<T>     _previous;
	readonly IOutputCacheStore _output;
	readonly IUserOutputKey    _key;

	protected OutputAware(IOperation<T> previous, IOutputCacheStore output, IUserOutputKey key)
	{
		_previous = previous;
		_output   = output;
		_key      = key;
	}

	public async ValueTask Get(T parameter)
	{
		await _previous.Off(parameter);
		await _output.EvictByTagAsync(_key.Get(parameter), CancellationToken.None).Off();
	}
}

public class OutputAware<TIn, T> : ISelecting<TIn, T> where TIn : IUserIdentity
{
	readonly ISelecting<TIn, T> _previous;
	readonly IOutputCacheStore  _output;
	readonly Func<T, bool>      _when;
	readonly IUserOutputKey     _key;

	protected OutputAware(ISelecting<TIn, T> previous, IOutputCacheStore output, IUserOutputKey key)
		: this(previous, output, _ => true, key) {}

	// ReSharper disable once TooManyDependencies
	protected OutputAware(ISelecting<TIn, T> previous, IOutputCacheStore output, Func<T, bool> when,
	                      IUserOutputKey key)
	{
		_previous = previous;
		_output   = output;
		_when     = when;
		_key      = key;
	}

	public async ValueTask<T> Get(TIn parameter)
	{
		var result = await _previous.Off(parameter);
		if (_when(result))
		{
			await _output.EvictByTagAsync(_key.Get(parameter), CancellationToken.None).Off();
		}

		return result;
	}
}