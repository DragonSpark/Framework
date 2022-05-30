﻿using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store;

public class MemoryStoreContext<TIn, TOut>
{
	public MemoryStoreContext(ISelect<TIn, TOut> subject, IMemoryCache memory)
	{
		Subject = subject;
		Memory  = memory;
	}

	protected ISelect<TIn, TOut> Subject { get; }

	protected IMemoryCache Memory { get; }

	public ConfiguredMemoryStoreContext<TIn, TOut> For(TimeSpan duration)
		=> new(Subject, Memory, new RelativeExpiration(duration));

	public ConfiguredMemoryStoreContext<TIn, TOut> For(Slide duration)
		=> new(Subject, Memory, new SlidingExpiration(duration.For));

	public ConfiguredMemoryStoreContext<TIn, TOut> ForProcessLifetime()
		=> new(Subject, Memory, EmptyCommand<ICacheEntry>.Default);
}