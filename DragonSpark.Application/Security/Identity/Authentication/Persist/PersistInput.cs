using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public readonly record struct PersistInput<T>(T User, Array<Claim> Claims);