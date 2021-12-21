using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class RefreshPrincipal : IAllocated<SecurityStampRefreshingPrincipalContext>
{
	readonly ICommand<Transfer> _copy;

	public RefreshPrincipal(ICommand<Transfer> copy) => _copy = copy;

	public Task Get(SecurityStampRefreshingPrincipalContext parameter)
	{
		var current = parameter.CurrentPrincipal.HasClaim(ExternalIdentity.Default);
		_copy.Execute(new(parameter.CurrentPrincipal, parameter.NewPrincipal));
		var @new    = parameter.NewPrincipal.HasClaim(ExternalIdentity.Default);
		// ReSharper disable once UnusedVariable -- for debugging.
		var stop    = !current || !@new;
		return Task.CompletedTask;
	}
}