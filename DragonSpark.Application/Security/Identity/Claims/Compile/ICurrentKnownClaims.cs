using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile
{
	public interface ICurrentKnownClaims : IArray<Claim> {}
}