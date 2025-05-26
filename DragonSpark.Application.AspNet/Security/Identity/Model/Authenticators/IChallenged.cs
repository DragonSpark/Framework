using DragonSpark.Model.Operations.Selection.Stop;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

public interface IChallenged<T> : IStopAware<ClaimsPrincipal, ChallengeResult<T>?> where T : IdentityUser;