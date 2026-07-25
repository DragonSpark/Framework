using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime;

public sealed class IdentifyingText : Result<string>
{
	public static IdentifyingText Default { get; } = new();

	IdentifyingText() : base(() => Guid.NewGuid().ToString()) {}

	public override string ToString() => Get();
}