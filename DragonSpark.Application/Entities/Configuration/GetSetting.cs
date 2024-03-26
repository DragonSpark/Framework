using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Application.Entities.Configuration;

sealed class GetSetting : EvaluateToSingleOrDefault<string, Setting>
{
	public GetSetting(IContexts contexts) : base(contexts, SelectSetting.Default) {}
}