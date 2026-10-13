namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed record ReviewSettings
{
	public bool EnablePrompt { get; set; } = true;
}