using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.Communication;

public sealed class AmbientBearer : Logical<string>
{
	public static AmbientBearer Default { get; } = new();

	AmbientBearer() {}
}