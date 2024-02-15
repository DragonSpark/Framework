namespace DragonSpark.Azure.Messaging;

public class MessagingConfiguration
{
	public string Namespace { get; set; } = default!;

	public string? Audience { get; set; }
}