using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Server.Security
{
	public interface IClaimsRepository
	{
		IEnumerable<Claim> DetermineClaims( string providerName, IDictionary<string, string> userData );
	}
}