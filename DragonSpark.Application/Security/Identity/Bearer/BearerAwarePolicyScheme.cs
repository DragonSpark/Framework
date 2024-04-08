using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class BearerAwarePolicyScheme : ICommand<PolicySchemeOptions>
{
	public static BearerAwarePolicyScheme Default { get; } = new();

	BearerAwarePolicyScheme() : this(PolicySelector.Default.Get) {}

	readonly Func<HttpContext, string?> _selector;

	public BearerAwarePolicyScheme(Func<HttpContext, string?> selector) => _selector = selector;

	public void Execute(PolicySchemeOptions parameter)
	{
		parameter.ForwardDefaultSelector = _selector;
	}
}