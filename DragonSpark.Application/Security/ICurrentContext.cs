using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.Security;

public interface ICurrentContext : IResult<HttpContext> {}