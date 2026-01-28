using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public readonly record struct IssueNonceInput(HttpContext Context, NoncePurpose Purpose);