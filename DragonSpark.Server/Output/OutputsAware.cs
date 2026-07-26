using DragonSpark.Application.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output;

public class OutputsAware<T> : Appending<Stop<T>> where T : notnull
{
	protected OutputsAware(IStopAware<T> previous, IOutputCacheStore output, params IOutputKey[] keys)
		: this(previous, new Evict<T>(output, keys)) {}

	protected OutputsAware(IStopAware<T> previous, IStopAware<T> evict) : base(previous, evict) {}
}

public class OutputsAware<TIn, T> : StopAware<TIn, T> where TIn : IUserIdentity
{
	protected OutputsAware(IStopAware<TIn, T> previous, IOutputCacheStore output, params IOutputKey[] keys)
		: this(new Evict<TIn, T>(previous, output, keys)) {}

	protected OutputsAware(IStopAware<TIn, T> previous, IStopAware<TIn> evict)
		: this(new Evict<TIn, T>(previous, evict)) {}

	protected OutputsAware(IStopAware<TIn, T> evict) : base(evict) {}
}