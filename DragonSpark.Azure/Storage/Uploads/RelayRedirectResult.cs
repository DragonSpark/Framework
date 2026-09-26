using DragonSpark.Text;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class RelayRedirectResult : RedirectResult, IText
{
	readonly string _url;
	readonly string _cacheControl;

	public RelayRedirectResult(string url) : this(url, DefaultRequestExpiration.Default) {}

	public RelayRedirectResult(string url, TimeSpan expire)
		: this(url, expire == TimeSpan.Zero
			            ? "no-cache, no-store, must-revalidate"
			            : $"private, max-age={expire.TotalSeconds:0}") {}

	public RelayRedirectResult(string url, string cacheControl) : base(url)
	{
		_url          = url;
		_cacheControl = cacheControl;
	}

	public override Task ExecuteResultAsync(ActionContext context)
	{
		context.HttpContext.Response.Headers.CacheControl = _cacheControl;
		return base.ExecuteResultAsync(context);
	}

	public string Get() => _url;
}