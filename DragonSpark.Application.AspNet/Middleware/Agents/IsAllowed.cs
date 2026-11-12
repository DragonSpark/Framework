using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class IsAllowed : AnyCondition<HttpRequest>
{
	public static IsAllowed Default { get; } = new();

	IsAllowed() : base(IsPlatformAgent.Default, IsInteractive.Default) {}
}