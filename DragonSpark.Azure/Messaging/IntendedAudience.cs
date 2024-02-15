namespace DragonSpark.Azure.Messaging;

sealed class IntendedAudience : Text.Text
{
	public static IntendedAudience Default { get; } = new();

	IntendedAudience() : base(nameof(IntendedAudience)) {}
}