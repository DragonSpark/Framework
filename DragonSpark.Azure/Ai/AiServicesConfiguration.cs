namespace DragonSpark.Azure.Ai;

public sealed class AiServicesConfiguration
{
	public string Address { get; set; } = null!;
	public string Key { get; set; } = null!;
	public string ImageModel { get; set; } = "dall-e-3";
}