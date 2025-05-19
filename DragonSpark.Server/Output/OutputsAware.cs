using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.OutputCaching;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Output;

public class OutputsAware<T> : IStopAware<T> where T : IUserIdentity
{
	readonly IOperation<Stop<T>> _previous;
	readonly IOutputCacheStore   _output;
	readonly Array<IOutputKey>   _keys;

	protected OutputsAware(IOperation<Stop<T>> previous, IOutputCacheStore output, params IOutputKey[] keys)
	{
		_previous = previous;
		_output   = output;
		_keys     = keys;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		await _previous.Off(parameter);
		foreach (var key in _keys.Open())
		{
			var tag = key is IUserOutputKey user ? user.Get(parameter.Subject) : key.Get(None.Default);
			await _output.EvictByTagAsync(tag, parameter).Off();
		}
	}
}
public class OutputsAware<TIn, T> : IStopAware<TIn, T> where TIn : IUserIdentity
{
	readonly ISelecting<Stop<TIn>, T> _previous;
	readonly IOutputCacheStore        _output;
	readonly Func<T, bool>            _when;
	readonly Array<IOutputKey>        _keys;

	protected OutputsAware(ISelecting<Stop<TIn>, T> previous, IOutputCacheStore output, params IOutputKey[] keys)
		: this(previous, output, _ => true, keys) {}

	// ReSharper disable once TooManyDependencies
	protected OutputsAware(ISelecting<Stop<TIn>, T> previous, IOutputCacheStore output, Func<T, bool> when,
	                       params IOutputKey[] keys)
	{
		_previous = previous;
		_output   = output;
		_when     = when;
		_keys     = keys;
	}

	public async ValueTask<T> Get(Stop<TIn> parameter)
	{
		var result = await _previous.Off(parameter);
		if (_when(result))
		{
			foreach (var key in _keys.Open())
			{
				var tag = key is IUserOutputKey user ? user.Get(parameter.Subject) : key.Get(None.Default);
				await _output.EvictByTagAsync(tag, parameter).Off();
			}
		}

		return result;
	}
}