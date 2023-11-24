namespace DragonSpark.Application.Testing.Objects;

sealed class ClaimNamespace : Text.Text
{
	public static ClaimNamespace Default { get; } = new();

	ClaimNamespace() : base("urn:testing") {}
}