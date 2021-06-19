using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims
{
	public interface IClaimAction : ICommand<ClaimActionCollection> {}
}