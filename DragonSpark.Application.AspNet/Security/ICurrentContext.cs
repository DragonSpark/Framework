using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security;

public interface ICurrentContext : IResult<HttpContext>;