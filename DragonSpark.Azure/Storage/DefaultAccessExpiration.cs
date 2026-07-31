using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage;

public sealed class DefaultAccessExpiration : Instance<TimeSpan>
{
	public static DefaultAccessExpiration Default { get; } = new();

	DefaultAccessExpiration() : base(TimeSpan.FromMinutes(10)) {}
}