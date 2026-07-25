namespace DragonSpark.Contracts.Messaging;

public readonly record struct DurableMessageProperties(
	Guid? Identifier,
	string Message,
	string Destination,
	TimeSpan? Visibility = null,
	TimeSpan? Life = null
)
{
	// ReSharper disable once TooManyDependencies
	public DurableMessageProperties(Guid Identifier, string Destination, TimeSpan? Visibility = null,
	                                    TimeSpan? Life = null)
		: this(Identifier, Identifier.ToString(), Destination, Visibility, Life) {}
}