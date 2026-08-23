namespace DragonSpark.Entra;

public sealed class DefaultSectionName : Text.Text
{
	public static DefaultSectionName Default { get; } = new();

	DefaultSectionName() : base("EntraAuthentication") {}
}