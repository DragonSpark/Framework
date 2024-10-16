using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

public readonly record struct GenerateImageInput(string Prompt, GeneratedImageSize Size, GeneratedImageFormat Format)
{
	public GenerateImageInput(string prompt) : this(prompt, GeneratedImageFormat.Bytes) {}

	public GenerateImageInput(string prompt, GeneratedImageFormat format)
		: this(prompt, GeneratedImageSize.W1024xH1024, format) {}
}