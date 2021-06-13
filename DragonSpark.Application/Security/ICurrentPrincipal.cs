using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public interface ICurrentPrincipal : IResult<ClaimsPrincipal> {}
}