using DragonSpark.Model.Results;
using OpenAI;
using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

sealed class ImageClients : IResult<ImageClient>
{
	readonly OpenAIClient _client;
	readonly string       _model;

	public ImageClients(OpenAIClient client, AiServicesConfiguration configuration)
		: this(client, configuration.ImageModel) {}

	public ImageClients(OpenAIClient client, string model)
	{
		_client = client;
		_model  = model;
	}

	public ImageClient Get() => _client.GetImageClient(_model);
}