using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	public interface IClaims : ISelect<Login, IEnumerable<Claim>> {}
}