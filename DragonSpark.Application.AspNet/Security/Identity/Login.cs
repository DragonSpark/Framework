using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed record Login<T>(ExternalLoginInfo Information, T User);