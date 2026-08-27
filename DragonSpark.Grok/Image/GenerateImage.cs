using System.Net.Http.Json;
using System.Text.Json;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Grok.Chat;
using DragonSpark.Model.Operations;

namespace DragonSpark.Grok.Image;

sealed class GenerateImage : IGenerateImage
{
	readonly Func<HttpClient>      _client;
	readonly JsonSerializerOptions _options;

	public GenerateImage(IHttpClientFactory factory)
		: this(Start.A.Selection<string, HttpClient>(factory.CreateClient)
		            .Then()
		            .Bind(RegistrationName.Default.Get),
		       ApiOptions.Default) {}

	[Candidate(false)]
	public GenerateImage(Func<HttpClient> client, JsonSerializerOptions options)
	{
		_client  = client;
		_options = options;
	}

	public async ValueTask<Uri> Get(Stop<ImageGenerationInput> parameter)
	{
		var (input, stop) = parameter;

		using var client = _client();
		var       post   = await client.PostAsJsonAsync("images/generations", input, _options, stop).Off();
		
		post.EnsureSuccessStatusCode();

		var response = await post.Content.ReadFromJsonAsync<ImageGenerationResponsePayload>(_options, stop).Off();
		var data     = response.Verify().Data[0];

		return data.Url.Verify("Image URL returned from Grok was null.");
	}
}