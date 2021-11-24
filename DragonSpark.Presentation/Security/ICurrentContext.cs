using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Security;

public interface ICurrentContext : IResult<HttpContext> {}