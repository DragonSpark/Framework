using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Application.Entities.Configuration;

sealed class GetSetting : EvaluateToSingleOrDefault<string, Setting>
{
	public GetSetting(IStandardScopes scopes) : base(scopes, SelectSetting.Default) {}
}