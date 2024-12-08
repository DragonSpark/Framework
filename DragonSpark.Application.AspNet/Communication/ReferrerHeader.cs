namespace DragonSpark.Application.AspNet.Communication;

public sealed class ReferrerHeader : Header
{
	public static ReferrerHeader Default { get; } = new();

	ReferrerHeader() : base("Referer") {}
}