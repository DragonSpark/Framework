using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

public interface IExtractClaims : IArray<ClaimsPrincipal, Claim>;