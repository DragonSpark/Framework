namespace DragonSpark.Presentation.Components.State;

public sealed class UpdateActivityReceiver : UpdateActivityReceiverBase
{
	public static UpdateActivityReceiver Default { get; } = new();

	UpdateActivityReceiver() : base(false) {}
}