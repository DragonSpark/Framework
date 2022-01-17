using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public readonly record struct LogClaimsInput(string UserName, Array<Claim> Claims);