using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Actions
{
	public interface IClaimAction : ICommand<ClaimActionCollection> {}
}