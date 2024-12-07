using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public readonly record struct PersistInput<T>(T User, IEnumerable<Claim> Claims);