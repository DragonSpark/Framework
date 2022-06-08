using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class RefreshPrincipal : IAllocated<SecurityStampRefreshingPrincipalContext>
{
	readonly ICommand<Transfer> _copy;

	public RefreshPrincipal(CopyKnownClaims copy) => _copy = copy;

	public Task Get(SecurityStampRefreshingPrincipalContext parameter)
	{
		_copy.Execute(new(parameter.CurrentPrincipal, parameter.NewPrincipal));
		return Task.CompletedTask;
	}
}