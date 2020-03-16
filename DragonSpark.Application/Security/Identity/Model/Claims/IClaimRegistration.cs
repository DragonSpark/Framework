using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.Model.Claims
{
	public interface IClaimRegistration : IExternalClaim, IResult<string> {}
}