using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed record Challenged(ExternalLoginInfo Login, string Origin);