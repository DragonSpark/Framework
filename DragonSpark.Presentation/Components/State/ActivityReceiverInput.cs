using DragonSpark.Application.Runtime.Operations;

namespace DragonSpark.Presentation.Components.State;

public readonly record struct ActivityReceiverInput(
	string? Message = null,
	ITokenHandle? Handle = null,
	bool ForceRefresh = false)
{
	public static ActivityReceiverInput Default { get; } = new();

	public static ActivityReceiverInput WithRefresh { get; } = new(ForceRefresh: true);
}