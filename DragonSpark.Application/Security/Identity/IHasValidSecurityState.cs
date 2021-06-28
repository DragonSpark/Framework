using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public interface IHasValidSecurityState : IDepending<ClaimsPrincipal> {}
}