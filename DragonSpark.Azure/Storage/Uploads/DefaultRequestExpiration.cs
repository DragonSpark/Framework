using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class DefaultRequestExpiration : Instance<TimeSpan>
{
	public static DefaultRequestExpiration Default { get; } = new();

	DefaultRequestExpiration() : base(DefaultAccessExpiration.Default.Get() / 2) {}
}