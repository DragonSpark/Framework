using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage;

public sealed class DefaultContentExpiration : Instance<TimeSpan>
{
	public static DefaultContentExpiration Default { get; } = new();

	DefaultContentExpiration() : base(TimeSpan.FromDays(30)) {}
}