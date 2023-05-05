using Microsoft.AspNetCore.Authentication;
using NetFabric.Hyperlinq;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public readonly record struct Composition(AuthenticationProperties? properties, Lease<Claim> claims);