using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

public readonly record struct GenerateImageInput(
	string Prompt,
	GeneratedImageSize Size,
	GeneratedImageFormat Format = GeneratedImageFormat.Bytes)
{
	public GenerateImageInput(string Prompt, GeneratedImageFormat Format = GeneratedImageFormat.Bytes)
		: this(Prompt, GeneratedImageSize.W1024xH1024, Format) {}
}