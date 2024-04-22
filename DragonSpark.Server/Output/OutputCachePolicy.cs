using DragonSpark.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Server.Output;

public class OutputCachePolicy : Text.Text, IOutputsPolicy
{
	readonly IOutputKey _key;
	readonly TimeSpan       _for;

	protected OutputCachePolicy(IOutputKey key) : this(key, DefaultExpiration.Default) {}

	protected OutputCachePolicy(IOutputKey key, TimeSpan @for) : base(key.Get())
	{
		_key = key;
		_for = @for;
	}

	ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
	{
		var http   = context.HttpContext;
		var method = http.Request.Method;
		var allow  = HttpMethods.IsGet(method) || HttpMethods.IsHead(method);
		var rules  = context.CacheVaryByRules;

		context.EnableOutputCaching        = true;
		context.AllowCacheLookup           = allow;
		context.AllowCacheStorage          = allow;
		context.AllowLocking               = true;
		context.ResponseExpirationTimeSpan = _for;
		rules.CacheKeyPrefix               = "";
		rules.QueryKeys                    = "*";

		var tag = _key.Get(None.Default);
		context.Tags.Add(tag);

		return ValueTask.CompletedTask;
	}

	ValueTask IOutputCachePolicy.ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
		=> ValueTask.CompletedTask;

	ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
	{
		context.AllowCacheStorage = true;
		return ValueTask.CompletedTask;
	}
}