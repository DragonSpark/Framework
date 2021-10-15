using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public interface IAuthenticationValidation : IDepending<ClaimsPrincipal> {}