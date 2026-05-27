using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public interface IAddClaim : IStopAware<ClaimInput, IdentityResult>;