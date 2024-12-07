namespace DragonSpark.Application.Compose;

public sealed class DefaultApplicationProfile : ApplicationProfile
{
	public static DefaultApplicationProfile Default { get; } = new();

	DefaultApplicationProfile() : base(_ => {}, _ => {}) {}
}