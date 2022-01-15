using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public readonly record struct LogAuthenticationInput(string UserName, Array<Claim> Claims);