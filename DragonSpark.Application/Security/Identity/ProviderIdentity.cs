namespace DragonSpark.Application.Security.Identity
{
	public readonly struct ProviderIdentity
	{
		public ProviderIdentity(string provider, string identity)
		{
			Provider = provider;
			Identity = identity;
		}

		public string Provider { get; }

		public string Identity { get; }

		public void Deconstruct(out string provider, out string identity)
		{
			provider = Provider;
			identity = Identity;
		}
	}
}