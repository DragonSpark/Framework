namespace DragonSpark.Azure.Messaging.Events;

sealed class Recipient : Text.Text
{
	public static Recipient Default { get; } = new();

	Recipient() : base(nameof(Recipient)) {}
}