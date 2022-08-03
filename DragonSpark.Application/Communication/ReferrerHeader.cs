namespace DragonSpark.Application.Communication;

public sealed class ReferrerHeader : Header
{
	public static ReferrerHeader Default { get; } = new();

	ReferrerHeader() : base("Referer") {}
}