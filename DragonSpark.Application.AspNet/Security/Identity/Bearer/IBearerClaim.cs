using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public interface IBearerClaim : ISelect<ClaimsIdentity, Claim>;