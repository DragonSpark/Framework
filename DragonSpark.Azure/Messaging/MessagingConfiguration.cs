namespace DragonSpark.Azure.Messaging;

public class MessagingConfiguration
{
	public required string Namespace { get; set; }

	public string? Audience { get; set; }
}