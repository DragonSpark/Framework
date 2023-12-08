using DragonSpark.Application.Communication;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Environment.Browser;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class IsApplicationAgent : ICondition<HttpContext>
{
	public static IsApplicationAgent Default { get; } = new();

	IsApplicationAgent() : this(IsUserRequest.Default, IsKnownWarmupAgent.Default.Then().Inverse().Out()) {}

	readonly ICondition<HttpRequest>       _user;
	readonly ICondition<IHeaderDictionary> _request;

	public IsApplicationAgent(ICondition<HttpRequest> user, ICondition<IHeaderDictionary> request)
	{
		_user    = user;
		_request = request;
	}

	public bool Get(HttpContext parameter) => _user.Get(parameter.Request) && _request.Get(parameter.Request.Headers);
}