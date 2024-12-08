using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

sealed class GetSetting : EvaluateToSingleOrDefault<string, Setting>
{
	public GetSetting(IScopes scopes) : base(scopes, SelectSetting.Default) {}
}