namespace DragonSpark.Application.Security.Identity
{
	public sealed class DisplayName : Text.Text
	{
		public static DisplayName Default { get; } = new DisplayName();

		DisplayName() : base($"{ClaimNamespace.Default}:displayname") {}
	}

	public sealed class ExternalIdentity : Text.Text
	{
		public static ExternalIdentity Default { get; } = new ();

		ExternalIdentity() : base($"{ClaimNamespace.Default}:identity") {}
	}
}