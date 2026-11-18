using DragonSpark.Application.Diagnostics;

namespace DragonSpark.Application.AspNet.Diagnostics;

public sealed class IgnoredApplicationException : ProcessedExceptionCondition
{
	public static IgnoredApplicationException Default { get; } = new();

	IgnoredApplicationException() : base(IgnoreException.Default.Get) {}
}