using System.Security.Claims;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Identity.Bearer;

public interface IBearerClaim : ISelect<ClaimsIdentity, Claim>;