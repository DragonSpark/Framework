namespace DragonSpark.Application;

public sealed class DefaultUniqueResourceName : UniqueResourceName
{
	public static DefaultUniqueResourceName Default { get; } = new DefaultUniqueResourceName();

	DefaultUniqueResourceName() : base(Sluggy.Sluggy.DefaultSeparator, Sluggy.Sluggy.DefaultTranslationStrategy) {}
}