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

public class OutputCachePolicy<T> : Text.Text, IOutputsPolicy
{
	readonly static Func<T?, bool> Assigned = Is.Assigned<T?>();
	
	readonly Func<HttpContext, T?> _select;
	readonly IOutputKey<T>         _key;
	readonly TimeSpan              _for;

	protected OutputCachePolicy(Func<HttpContext, T?> select, IOutputKey<T> key)
		: this(select, key, DefaultExpiration.Default) {}

	protected OutputCachePolicy(Func<HttpContext, T?> select, IOutputKey<T> key, TimeSpan @for) : base(key.Name)
	{
		_select = select;
		_key    = key;
		_for    = @for;
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
		context.Tags.Add(_key.Get());
		var value = _select(http);
		if (Assigned(value))
		{
			var tag = _key.Get(value.Verify());
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