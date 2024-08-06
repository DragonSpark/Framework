using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.OutputCaching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Server.Output;

public class OutputsAware<T> : IOperation<T> where T : IUserIdentity
{
	readonly IOperation<T>         _previous;
	readonly IOutputCacheStore     _output;
	readonly Array<IUserOutputKey> _keys;

	protected OutputsAware(IOperation<T> previous, IOutputCacheStore output, params IUserOutputKey[] keys)
	{
		_previous = previous;
		_output   = output;
		_keys     = keys;
	}

	public async ValueTask Get(T parameter)
	{
		await _previous.Await(parameter);
		foreach (var key in _keys.Open())
		{
			await _output.EvictByTagAsync(key.Get(parameter), CancellationToken.None).Await();
		}
	}
}

public class OutputsAware<TIn, T> : ISelecting<TIn, T> where TIn : IUserIdentity
{
	readonly ISelecting<TIn, T>    _previous;
	readonly IOutputCacheStore     _output;
	readonly Func<T, bool>         _when;
	readonly Array<IUserOutputKey> _keys;

	protected OutputsAware(ISelecting<TIn, T> previous, IOutputCacheStore output, params IUserOutputKey[] keys)
		: this(previous, output, _ => true, keys) {}

	// ReSharper disable once TooManyDependencies
	protected OutputsAware(ISelecting<TIn, T> previous, IOutputCacheStore output, Func<T, bool> when,
	                       params IUserOutputKey[] keys)
	{
		_previous = previous;
		_output   = output;
		_when     = when;
		_keys     = keys;
	}

	public async ValueTask<T> Get(TIn parameter)
	{
		var result = await _previous.Await(parameter);
		if (_when(result))
		{
			foreach (var key in _keys.Open())
			{
				await _output.EvictByTagAsync(key.Get(parameter), CancellationToken.None).Await();
			}
		}

		return result;
	}
}