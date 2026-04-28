using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

public readonly record struct GenerateImageInput(string Prompt, GeneratedImageSize Size, GeneratedImageQuality Quality)
{
	public GenerateImageInput(string prompt) : this(prompt, GeneratedImageSize.W1024xH1024) {}

	public GenerateImageInput(string Prompt, GeneratedImageSize Size)
		: this(Prompt, Size, GeneratedImageQuality.LowQuality) {}
}