using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	interface IAppliedPrincipal : ISelect<ExternalLoginInfo, ClaimsPrincipal> {}
}