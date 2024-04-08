using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class PolicySelector : ISelect<HttpContext, string?>
{
	public static PolicySelector Default { get; } = new();

	PolicySelector()
		: this("Bearer ", JwtBearerDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme) {}

	readonly string _key;
	readonly string _scheme;
	readonly string _cookie;

	public PolicySelector(string key, string scheme, string cookie)
	{
		_key    = key;
		_scheme = scheme;
		_cookie = cookie;
	}

	public string Get(HttpContext parameter)
	{
		var header = parameter.Request.Headers.Authorization;
		var result = !string.IsNullOrEmpty(header) && header.ToString().StartsWith(_key) ? _scheme : _cookie;
		return result;
	}
}