using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile
{
	public interface IClaims : ISelect<Login, IEnumerable<Claim>> {}
}