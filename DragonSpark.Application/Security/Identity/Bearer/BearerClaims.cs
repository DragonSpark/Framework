using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class BearerClaims : ISelect<ClaimsIdentity, IEnumerable<Claim>>
{
	readonly string[]            _known;
	readonly Array<IBearerClaim> _claims;

	public BearerClaims(IEnumerable<IBearerClaim> direct)
		: this(new[] { ClaimTypes.NameIdentifier, ClaimTypes.Name }, direct.ToArray()) {}

	public BearerClaims(string[] known, Array<IBearerClaim> claims)
	{
		_known  = known;
		_claims = claims;
	}

	public IEnumerable<Claim> Get(ClaimsIdentity parameter)
	{
		var known  = _known.Select(parameter.FindFirst).Where(x => x != null).Select(x => x.Verify());
		var claims = _claims.Open().Introduce(parameter).Select(x => x.Item1.Get(x.Item2));
		return known.Concat(claims);
	}
}