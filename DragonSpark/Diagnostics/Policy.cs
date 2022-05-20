using DragonSpark.Model.Results;
using Polly;
using System;

namespace DragonSpark.Diagnostics;

public class Policy : DeferredSingleton<IAsyncPolicy>
{
	protected Policy(Func<IAsyncPolicy> source) : base(source) {}

	protected Policy(Lazy<IAsyncPolicy> source) : base(source) {}
}