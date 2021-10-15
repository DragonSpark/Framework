using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

public interface IChallenged<T> : ISelecting<ClaimsPrincipal, ChallengeResult<T>?> where T : IdentityUser {}