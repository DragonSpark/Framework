using Azure;
using Azure.AI.OpenAI;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Ai;

public sealed class AiServicesClient : Instance<AzureOpenAIClient>
{
	public AiServicesClient(AiServicesConfiguration configuration) : this(configuration, new(configuration.Key)) {}

	public AiServicesClient(AiServicesConfiguration configuration, AzureKeyCredential credential)
		: base(new(new(configuration.Address), credential)) {}
}