using Azure;
using Azure.AI.OpenAI;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Images;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Ai;

class Class1;

public sealed class AiServicesConfiguration
{
	public string Address { get; set; } = default!;
	public string Key { get; set; } = default!;
	public string ImageModel { get; set; } = "dall-e-3";
}

public sealed class AiServicesClient : Instance<AzureOpenAIClient>
{
	public AiServicesClient(AiServicesConfiguration configuration) : this(configuration, new(configuration.Key)) {}

	public AiServicesClient(AiServicesConfiguration configuration, AzureKeyCredential credential)
		: base(new(new(configuration.Address), credential)) {}
}

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<AiServicesConfiguration>()
		         .Start<OpenAIClient>()
		         .Use<AiServicesClient>()
		         .Singleton()
		         //
		         .Then.Start<ImageClient>()
		         .Use<ImageClients>()
		         .Singleton();
	}
}

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

public sealed class GenerateImage : ISelecting<Token<string>, GeneratedImage>
{
	readonly ImageClient            _client;
	readonly ImageGenerationOptions _options;

	public GenerateImage(ImageClient client) : this(client, new() { Size = GeneratedImageSize.W1024xH1024 }) {}

	public GenerateImage(ImageClient client, ImageGenerationOptions options)
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