namespace DragonSpark.Application.Security
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
	}
}