using Microsoft.AspNetCore.Identity;
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

		public Claim Get(ExternalLoginInfo parameter) => new Claim(_key, parameter.Principal.FindFirstValue(_source));
	}
}