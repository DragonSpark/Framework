using Polly;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics;

public sealed class IgnorePolicy : IPolicy
{
	public static IgnorePolicy Default { get; } = new();

	IgnorePolicy() {}

	public IAsyncPolicy Get(PolicyBuilder parameter) => parameter.FallbackAsync(_ => Task.CompletedTask);
}

public sealed class IgnorePolicy<T> : IPolicy<T>
{
	public static IgnorePolicy<T> Default { get; } = new();

	IgnorePolicy() {}

	public IAsyncPolicy<T> Get(PolicyBuilder<T> parameter) => parameter.FallbackAsync(_ => default);
}