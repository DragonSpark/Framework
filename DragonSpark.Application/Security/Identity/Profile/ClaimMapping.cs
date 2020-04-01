using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public sealed class ClaimMapping : IClaimMapping
	{
		readonly string _key, _source;

		public ClaimMapping(string key, string source)
		{
			_key    = key;
			_source = source;
		}

		public Claim Get(ClaimsPrincipal parameter) => new Claim(_key, parameter.FindFirstValue(_source));
	}
}