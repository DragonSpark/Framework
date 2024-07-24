using DragonSpark.Application.Runtime.Operations;

namespace DragonSpark.Presentation.Components.State;

public readonly record struct ActivityReceiverInput(string? Message = null, ITokenHandle? Handle = null)
{
	public static ActivityReceiverInput Default { get; } = new();
}