using DragonSpark.Model.Sequences.Collections;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

public sealed class UserNamesRequirement(params string[] names) : Contains(names), IAuthorizationRequirement;