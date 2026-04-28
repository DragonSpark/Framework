using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

public readonly record struct GenerateImageInput(string Prompt, GeneratedImageSize Size)
{
	public GenerateImageInput(string prompt) : this(prompt, GeneratedImageSize.W1024xH1024) {}
}