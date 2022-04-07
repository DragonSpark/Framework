using System;
using System.Security.Claims;

namespace DragonSpark.Server.Requests;

public readonly record struct Input(ClaimsPrincipal Principal, Guid Identity);

public readonly record struct Input<T>(ClaimsPrincipal Principal, T Argument);