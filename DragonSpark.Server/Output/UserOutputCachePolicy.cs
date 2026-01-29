using DragonSpark.Application.AspNet;
using DragonSpark.Application.Security;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Server.Output;

public class UserOutputCachePolicy : Text.Text, IOutputsPolicy
{
	readonly IUserOutputKey _key;
	readonly TimeSpan       _for;

	protected UserOutputCachePolicy(IUserOutputKey key) : this(key, DefaultExpiration.Default) {}

	protected UserOutputCachePolicy(IUserOutputKey key, TimeSpan @for) : base(key.Get())
	{
		_key = key;
		_for = @for;
	}

	async ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
	{
		var http   = context.HttpContext;
		var method = http.Request.Method;
		var post   = HttpMethods.IsPost(method);
		var allow  = HttpMethods.IsGet(method) || HttpMethods.IsHead(method) || post;
		var rules  = context.CacheVaryByRules;
		var number = http.User.Number() ?? 0;
		context.EnableOutputCaching        = true;
		context.AllowCacheLookup           = allow;
		context.AllowCacheStorage          = allow;
		context.AllowLocking               = true;
		context.ResponseExpirationTimeSpan = _for;
		rules.CacheKeyPrefix               = $"{number}_";

		if (post)
		{
			http.Request.EnableBuffering();
			using var reader = new StreamReader(http.Request.Body, leaveOpen: true);
			var       body   = await reader.ReadToEndAsync(cancellationToken).Off();
			http.Request.Body.Position = 0;
			context.CacheVaryByRules.VaryByValues.Add("body", HexHash.Default.Get(body));
		}
		else
		{
			rules.QueryKeys = "*";
		}

		var tag = _key.Get(new UserIdentity(number));
		context.Tags.Add(tag);
	}

	ValueTask IOutputCachePolicy.ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
		=> ValueTask.CompletedTask;

	ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
	{
		context.AllowCacheStorage = true;
		return ValueTask.CompletedTask;
	}
}