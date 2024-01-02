namespace DragonSpark.Azure.Events;

sealed class Recipient : Text.Text
{
	public static Recipient Default { get; } = new();

	Recipient() : base(nameof(Recipient)) {}
}