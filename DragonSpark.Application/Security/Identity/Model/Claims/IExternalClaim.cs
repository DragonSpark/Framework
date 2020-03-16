using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model.Claims
{
	public interface IExternalClaim : ISelect<ExternalLoginInfo, Claim> {}
}