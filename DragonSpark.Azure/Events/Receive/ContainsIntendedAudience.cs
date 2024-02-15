namespace DragonSpark.Azure.Events.Receive;

public sealed class ContainsIntendedAudience : ContainsPropertyValue
{
	public ContainsIntendedAudience(string? audience) : base(IntendedAudience.Default, audience) {}
}