using Microsoft.AspNetCore.Authentication;
using NetFabric.Hyperlinq;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Persist;

public readonly record struct PersistMetadataInput<T>(T User, AuthenticationProperties? Metadata, Lease<Claim> Claims);