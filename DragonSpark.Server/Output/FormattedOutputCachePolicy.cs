using DragonSpark.Compose;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output;

public class OutputCachePolicy : Text.Text, IOutputsPolicy
{
	readonly IOutputKey _key;
	readonly TimeSpan   _for;

	protected OutputCachePolicy(IOutputKey key) : this(key, DefaultExpiration.Default) {}

	protected OutputCachePolicy(IOutputKey key, TimeSpan @for) : base(key.Name)
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
		rules.QueryKeys                    = "*";
		var tag = _key.Get();
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

public class FormattedOutputCachePolicy : Text.Text, IOutputsPolicy
{
	readonly Func<HttpContext, string?> _tag;
	readonly string                     _key;
	readonly TimeSpan                   _for;

	protected FormattedOutputCachePolicy(Func<HttpContext, string?> tag, IOutputKey key)
		: this(tag, key, DefaultExpiration.Default) {}

	protected FormattedOutputCachePolicy(Func<HttpContext, string?> tag, IOutputKey key, TimeSpan @for)
		: this(tag, key.Get(), @for) {}

	protected FormattedOutputCachePolicy(Func<HttpContext, string?> tag, string key, TimeSpan @for) : base(key)
	{
		_tag = tag;
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
		rules.QueryKeys                    = "*";
		//context.Tags.Add(_key);
		var input = _tag(http);
		if (input.IsAssigned())
		{
			var tag = OutputKeyFormatter.Default.Get(new(_key, input));
			context.Tags.Add(tag);
		}

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