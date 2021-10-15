using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

public interface ICurrentPrincipal : IResult<ClaimsPrincipal> {}