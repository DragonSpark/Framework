using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class ErrorResponse : Instance<ResponseResult>
{
    public static ErrorResponse Default { get; } = new();

    ErrorResponse() : base(new("""{"error":"handoff_unavailable"}""", StatusCodes.Status502BadGateway)) {}
}