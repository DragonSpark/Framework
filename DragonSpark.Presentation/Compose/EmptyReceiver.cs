using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Compose;

public sealed class EmptyReceiver : Instance<object>
{
	public static EmptyReceiver Default { get; } = new();

	EmptyReceiver() : base(new()) {}
}