using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Requests.Warmup;

/// <summary>
/// ATTRIBUTION: https://haacked.com/archive/2020/09/28/azure-swap-with-warmup-aspnetcore/#6b495a64
/// </summary>
public sealed class IsWarmupRequest : ICondition<HttpContext>
{
	public static IsWarmupRequest Default { get; } = new();

	IsWarmupRequest() : this(IsKnownUserAgent.Default, IsWarmupAddress.Default) { }

	readonly ICondition<IHeaderDictionary> _agent;
	readonly ICondition<HttpContext>       _address;

	public IsWarmupRequest(ICondition<IHeaderDictionary> agent, ICondition<HttpContext> address)
	{
		_agent   = agent;
		_address = address;
	}

	public bool Get(HttpContext parameter) => _agent.Get(parameter.Request.Headers) && _address.Get(parameter);
}