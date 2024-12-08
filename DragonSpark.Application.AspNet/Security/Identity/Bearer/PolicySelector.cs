using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class PolicySelector : ISelect<HttpContext, string?>
{
	public static PolicySelector Default { get; } = new();

	PolicySelector()
		: this("Bearer ", JwtBearerDefaults.AuthenticationScheme, IdentityConstants.ApplicationScheme) {}

	readonly string _key;
	readonly string _scheme;
	readonly string _previous;

	public PolicySelector(string key, string scheme, string previous)
	{
		_key    = key;
		_scheme = scheme;
		_previous = previous;
	}

	public string Get(HttpContext parameter)
	{
		var header = parameter.Request.Headers.Authorization;
		var result = !string.IsNullOrEmpty(header) && header.ToString().StartsWith(_key) ? _scheme : _previous;
		return result;
	}
}