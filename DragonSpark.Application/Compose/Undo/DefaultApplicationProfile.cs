namespace DragonSpark.Application.Compose.Undo;

public sealed class DefaultApplicationProfile : ApplicationProfile
{
	public static DefaultApplicationProfile Default { get; } = new();

	DefaultApplicationProfile() : base(_ => {}) {}
}