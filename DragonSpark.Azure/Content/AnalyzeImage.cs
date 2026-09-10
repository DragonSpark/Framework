using Azure;
using Azure.AI.ContentSafety;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Content;

public sealed class AnalyzeImage : IStopAware<AnalyzeImageOptions, ModerationResult>
{
	readonly ContentSafetyClient _client;
	readonly byte                _allowed;

	public AnalyzeImage(ContentSafetyClient client, ContentSafetyConfiguration configuration)
		: this(client, configuration.AllowedSafety) {}

	public AnalyzeImage(ContentSafetyClient client, byte allowed)
	{
		_client  = client;
		_allowed = allowed;
	}

	public async ValueTask<ModerationResult> Get(Stop<AnalyzeImageOptions> parameter)
	{
		var (subject, stop) = parameter;
		try
		{
			var response = await _client.AnalyzeImageAsync(subject, stop).Off();
			var result   = response.Value;

			var violations = result.CategoriesAnalysis.Where(c => c.Severity > _allowed)
			                       .Select(c => $"{c.Category}: Severity {c.Severity}")
			                       .ToList();

			return new (violations);
		}
		catch (RequestFailedException ex)
		{
			throw new InvalidOperationException($"Content Safety API call failed: {ex.Message}", ex);
		}
	}
}