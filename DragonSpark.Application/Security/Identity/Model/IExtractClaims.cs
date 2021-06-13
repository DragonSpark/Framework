using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	public interface IExtractClaims : IArray<ClaimsPrincipal, Claim> {}
}