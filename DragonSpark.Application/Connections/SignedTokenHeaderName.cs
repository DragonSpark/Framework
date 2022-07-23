namespace DragonSpark.Application.Connections;

sealed class SignedTokenHeaderName : Text.Text
{
	public static SignedTokenHeaderName Default { get; } = new();

	SignedTokenHeaderName() : base("x-dragonspark-token") {}
}