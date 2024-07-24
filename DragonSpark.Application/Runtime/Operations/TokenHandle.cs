﻿using DragonSpark.Model.Results;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class TokenHandle : ITokenHandle
{
	readonly IMutable<CancellationTokenSource?> _store;
	readonly IResult<CancellationTokenSource>   _source;

	public TokenHandle() : this(new Variable<CancellationTokenSource>(new CancellationTokenSource())) {}

	public TokenHandle(IMutable<CancellationTokenSource?> store) : this(store, new TokenSource(store)) {}

	public TokenHandle(IMutable<CancellationTokenSource?> store, IResult<CancellationTokenSource> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask Get()
	{
		var source = _store.Get();
		if (source is not null)
		{
			_store.Execute(null);
			await source.CancelAsync().ConfigureAwait(false);
		}
	}

	public CancellationToken Token => _source.Get().Token;
}