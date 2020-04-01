using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public interface IClaimMapping : ISelect<ClaimsPrincipal, Claim> {}
}