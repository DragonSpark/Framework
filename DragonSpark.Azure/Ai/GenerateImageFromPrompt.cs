using DragonSpark.Composition;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection;
using OpenAI.Images;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Ai;

public sealed class GenerateImageFromPrompt : ISelecting<Token<string>, GeneratedImage>
{
	readonly ImageClient            _client;
	readonly ImageGenerationOptions _options;

	public GenerateImageFromPrompt(ImageClient client)
		: this(client, new() { ResponseFormat = GeneratedImageFormat.Bytes, Size = GeneratedImageSize.W1024xH1024 }) {}

	[Candidate(false)]
	public GenerateImageFromPrompt(ImageClient client, ImageGenerationOptions options)
	{
		_client  = client;
		_options = options;
	}

	public async ValueTask<GeneratedImage> Get(Token<string> parameter)
	{
		var response = await _client.GenerateImageAsync(parameter, _options, parameter.Item);
		return response.Value;
	}
}