namespace DragonSpark.Azure.Ai;

public sealed class AiServicesConfiguration
{
	public required string Address { get; set; }
	public required string Key { get; set; }
	public string ImageModel { get; set; } = DefaultImageModel.Default;
}