using DragonSpark.Model.Results;
using Polly;
using System;

namespace DragonSpark.Diagnostics;

public class Policy : Deferred<IAsyncPolicy>
{
	protected Policy(Func<IAsyncPolicy> source) : base(source) {}

	protected Policy(Lazy<IAsyncPolicy> source) : base(source) {}
}

public class Policy<T> : Deferred<IAsyncPolicy<T>>
{
	protected Policy(Func<IAsyncPolicy<T>> source) : base(source) {}

	protected Policy(Lazy<IAsyncPolicy<T>> source) : base(source) {}
}