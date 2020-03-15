namespace DragonSpark.Application.Security.Identity.Profile {
	public sealed class DisplayName : Text.Text
	{
		public static DisplayName Default { get; } = new DisplayName();

		DisplayName() : base($"{ClaimNamespace.Default}:displayname") {}
	}
}