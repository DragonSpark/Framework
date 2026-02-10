using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public readonly record struct InterceptLoginRequestInput(RequestDelegate Previous, HttpContext Context);