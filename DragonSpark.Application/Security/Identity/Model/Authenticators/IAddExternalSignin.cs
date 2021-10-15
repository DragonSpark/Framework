using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

public interface IAddExternalSignin : ISelecting<ClaimsPrincipal, IdentityResult?> {}