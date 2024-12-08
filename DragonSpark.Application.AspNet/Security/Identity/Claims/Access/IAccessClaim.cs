using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

public interface IAccessClaim<T> : ISelect<ClaimsPrincipal, Claim<T>>;