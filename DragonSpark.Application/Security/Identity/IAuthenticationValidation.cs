using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public interface IAuthenticationValidation : IDepending<ClaimsPrincipal> {}
}