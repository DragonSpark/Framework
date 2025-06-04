using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.Runtime.Objects;

sealed class AmbientTarget : Local<object>
{
	public static AmbientTarget Default { get; } = new();

	AmbientTarget() {}
}