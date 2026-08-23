namespace DragonSpark.Entra;

public sealed class EmailClaim : Text.Text
{
	public static EmailClaim Default { get; } = new();

	EmailClaim() : base("preferred_username") {}
}