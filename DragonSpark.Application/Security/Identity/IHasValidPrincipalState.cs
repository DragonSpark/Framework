using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

public interface IHasValidPrincipalState : IDepending<ClaimsPrincipal> {}