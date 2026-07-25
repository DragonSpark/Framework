using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public readonly record struct RewriteResponseInput(HttpResponse Response, MemoryStream Stream);