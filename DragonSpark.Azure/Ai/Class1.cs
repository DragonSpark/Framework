using Azure;
using Azure.AI.OpenAI;
using DragonSpark.Application.Connections.Events;
using DragonSpark.Composition;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Images;
using System.ClientModel;
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

public readonly record struct GenerateImageInput(
	string Prompt,
	GeneratedImageSize Size,
	GeneratedImageFormat Format = GeneratedImageFormat.Bytes)
{
	public GenerateImageInput(string Prompt, GeneratedImageFormat Format = GeneratedImageFormat.Bytes)
		: this(Prompt, GeneratedImageSize.W1024xH1024, Format) {}
}

public sealed class GenerateImage : ISelecting<Token<GenerateImageInput>, GeneratedImage>
{
	readonly ImageClient _client;

	public GenerateImage(ImageClient client) => _client = client;

	public async ValueTask<GeneratedImage> Get(Token<GenerateImageInput> parameter)
	{
		var ((prompt, size, format), item) = parameter;
		var response = await _client.GenerateImageAsync(prompt, new() { ResponseFormat = format, Size = size }, item);
		return response.Value;
	}
}

public sealed class DurableConnectionPolicy : DurableConnectionPolicyBase<ClientResultException>
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy() : base(DefaultRetryPolicy.Default) {}
}