using DragonSpark.Model.Results;

namespace DragonSpark.Server.Output;

public sealed class DefaultExpiration : Instance<TimeSpan>
{
	public static DefaultExpiration Default { get; } = new();

	DefaultExpiration() : base(TimeSpan.FromDays(1)) {}
}