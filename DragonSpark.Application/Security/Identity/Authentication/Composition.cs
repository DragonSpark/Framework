using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public readonly struct Composition
	{
		public Composition(AuthenticationProperties? properties, Array<Claim> claims)
		{
			Properties = properties;
			Claims     = claims;
		}

		public AuthenticationProperties? Properties { get; }

		public Array<Claim> Claims { get; }

		public void Deconstruct(out AuthenticationProperties? properties, out Array<Claim> claims)
		{
			properties = Properties;
			claims     = Claims;
		}
	}
}