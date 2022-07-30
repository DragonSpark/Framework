namespace DragonSpark.Application.Communication;

public sealed class RefererHeader : Header
{
	public static RefererHeader Default { get; } = new();

	RefererHeader() : base("Referer") {}
}