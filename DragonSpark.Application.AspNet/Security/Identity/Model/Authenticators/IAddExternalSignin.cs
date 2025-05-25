using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

public interface IAddExternalSignin : IStopAware<ClaimsPrincipal, IdentityResult?>;