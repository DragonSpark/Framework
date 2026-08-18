using DragonSpark.Text;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class RelayRedirectResult : RedirectResult, IText
{
	readonly string   _url;
	readonly TimeSpan _expire;

	public RelayRedirectResult(string url) : this(url, DefaultRequestExpiration.Default) {}

	public RelayRedirectResult(string url, TimeSpan expire) : base(url)
	{
		_url    = url;
		_expire = expire;
	}

	public override void ExecuteResult(ActionContext context)
	{
		context.HttpContext.Response.Headers.CacheControl
			= _expire == TimeSpan.Zero
				  ? "no-cache, no-store, must-revalidate"
				  : $"private, max-age={_expire.TotalSeconds:0}";
		base.ExecuteResult(context);
	}

	public string Get() => _url;
}