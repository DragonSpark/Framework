using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

public interface IClaims : ISelect<Login, IEnumerable<Claim>>;