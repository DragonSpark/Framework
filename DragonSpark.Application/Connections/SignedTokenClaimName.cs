namespace DragonSpark.Application.Connections;

sealed class SignedTokenClaimName : Text.Text
{
	public static SignedTokenClaimName Default { get; } = new();

	SignedTokenClaimName() : base("DragonSpark-Token") {}
}