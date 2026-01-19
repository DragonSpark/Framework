using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public readonly record struct ResponseResult(string Content, int Code = StatusCodes.Status200OK);