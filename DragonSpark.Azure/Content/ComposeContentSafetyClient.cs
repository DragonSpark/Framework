using Azure;
using Azure.AI.ContentSafety;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Content;

sealed class ComposeContentSafetyClient : Instance<ContentSafetyClient>
{
	public ComposeContentSafetyClient(ContentSafetyConfiguration configuration)
		: base(new(configuration.Endpoint, new AzureKeyCredential(configuration.Key))) {}
}