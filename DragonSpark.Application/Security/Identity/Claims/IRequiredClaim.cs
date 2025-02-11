using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims;

public interface IRequiredClaim : ISelect<ClaimsPrincipal, string>;