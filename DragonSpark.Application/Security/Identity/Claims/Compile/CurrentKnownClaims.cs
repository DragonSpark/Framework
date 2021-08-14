using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile
{
	sealed class CurrentKnownClaims : ArrayResult<Claim>, ICurrentKnownClaims
	{
		public CurrentKnownClaims(IExtractClaims extract, ICurrentPrincipal current)
			: base(extract.Then().Bind(current).Select(x => x.Result())) {}
	}
}