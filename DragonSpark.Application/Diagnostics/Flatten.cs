using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Diagnostics;

public sealed class Flatten : IAlteration<Exception>
{
	public static Flatten Default { get; } = new();

	Flatten() {}

	public Exception Get(Exception parameter) => parameter is AggregateException x ? x.Flatten() : parameter;
}