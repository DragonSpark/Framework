using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public readonly record struct PersistMetadataInput<T>(T User, AuthenticationProperties? Metadata, Array<Claim> Claims);