using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public readonly struct Login
	{
		public Login(ClaimsPrincipal identity, string provider, string key)
		{
			Identity = identity;
			Provider = provider;
			Key      = key;
		}

		public ClaimsPrincipal Identity { get; }

		public string Provider { get; }
		public string Key { get; }

		public void Deconstruct(out ClaimsPrincipal identity, out string provider, out string key)
		{
			identity = Identity;
			provider = Provider;
			key      = Key;
		}
	}
}