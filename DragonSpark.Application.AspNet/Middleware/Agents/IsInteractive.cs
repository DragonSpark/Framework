using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class IsInteractive : ICondition<HttpRequest>
{
	public static IsInteractive Default { get; } = new();

	IsInteractive() : this(ChallengeStateKey.Default) {}

	readonly string _key;

	public IsInteractive(string key) => _key = key;

	public bool Get(HttpRequest parameter)
	{
		var referer  = parameter.Headers.Referer.ToString().IsAssigned();
		var cookies  = parameter.Cookies;
		var verified = cookies.ContainsKey(_key) || cookies.Keys.Any(k => k.Contains("Antiforgery"));
		return referer || verified;
	}
}