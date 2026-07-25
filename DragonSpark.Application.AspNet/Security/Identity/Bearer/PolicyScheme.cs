using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class PolicyScheme : ICommand<PolicySchemeOptions>
{
	readonly Func<HttpContext, string?> _selector;

	public PolicyScheme(IPolicySelector selector) : this(selector.Get) {}

	public PolicyScheme(Func<HttpContext, string?> selector) => _selector = selector;

	public void Execute(PolicySchemeOptions parameter)
	{
		parameter.ForwardDefaultSelector = _selector;
	}
}