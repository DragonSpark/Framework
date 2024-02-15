namespace DragonSpark.Azure.Events.Receive;

sealed class IntendedAudience : Text.Text
{
	public static IntendedAudience Default { get; } = new();

	IntendedAudience() : base(nameof(IntendedAudience)) {}
}