using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class RefreshPrincipal : IAllocated<SecurityStampRefreshingPrincipalContext>
	{
		readonly ICommand<(ClaimsPrincipal From, ClaimsPrincipal To)> _copy;

		public RefreshPrincipal(ICommand<(ClaimsPrincipal From, ClaimsPrincipal To)> copy) => _copy = copy;

		public Task Get(SecurityStampRefreshingPrincipalContext parameter)
		{
			_copy.Execute(parameter.CurrentPrincipal, parameter.NewPrincipal);
			return Task.CompletedTask;
		}
	}
}