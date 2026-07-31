namespace DragonSpark.Azure.Storage;

public readonly record struct RelayInput(
	string? Name,
	string? ContentType,
	TimeSpan Start,
	TimeSpan Access,
	TimeSpan Content)
{
	public RelayInput(string? Name = null, string? ContentType = null)
		: this(DefaultAccessExpiration.Default, DefaultContentExpiration.Default, Name, ContentType) {}

	// ReSharper disable once TooManyDependencies
	public RelayInput(TimeSpan Access, TimeSpan Content, string? Name = null, string? ContentType = null)
		: this(Name, ContentType, TimeSpan.FromMinutes(-5), Access, Content) {}
}