using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface ICurrentPrincipal : IResult<ClaimsPrincipal>;