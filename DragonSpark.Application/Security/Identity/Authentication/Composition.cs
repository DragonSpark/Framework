using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public readonly record struct Composition(AuthenticationProperties? properties, Array<Claim> claims);