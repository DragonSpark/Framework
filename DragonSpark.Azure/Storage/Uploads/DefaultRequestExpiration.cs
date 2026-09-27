using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class DefaultRequestExpiration : Instance<TimeSpan>
{
	public static DefaultRequestExpiration Default { get; } = new();

	DefaultRequestExpiration() : this(DefaultAccessExpiration.Default) {}

	public DefaultRequestExpiration(TimeSpan access) : base(access / 2) {}
}