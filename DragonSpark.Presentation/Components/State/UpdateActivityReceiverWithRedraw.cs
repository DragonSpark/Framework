namespace DragonSpark.Presentation.Components.State;

sealed class UpdateActivityReceiverWithRedraw : UpdateActivityReceiverBase
{
	public static UpdateActivityReceiverWithRedraw Default { get; } = new();

	UpdateActivityReceiverWithRedraw() : base(true) {}
}