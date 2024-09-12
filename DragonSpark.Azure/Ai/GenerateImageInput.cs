using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

public readonly record struct GenerateImageInput(
	string Prompt,
	GeneratedImageSize Size,
	GeneratedImageFormat Format = GeneratedImageFormat.Bytes)
{
	public GenerateImageInput(string prompt, GeneratedImageFormat format = GeneratedImageFormat.Bytes)
		: this(prompt, GeneratedImageSize.W1024xH1024, format) {}
}