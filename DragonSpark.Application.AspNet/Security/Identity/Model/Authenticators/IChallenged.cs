using DragonSpark.Model.Operations.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

public interface IChallenged<T> : ISelecting<ClaimsPrincipal, ChallengeResult<T>?> where T : IdentityUser;