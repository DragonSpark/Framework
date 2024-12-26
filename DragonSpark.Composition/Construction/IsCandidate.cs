using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Composition.Construction;

sealed class IsCandidate : ICondition<ConstructorCandidate>
{
	public static IsCandidate Default { get; } = new();

	IsCandidate() {}

	public bool Get(ConstructorCandidate parameter)
		=> parameter.Constructor is { IsPublic: true, IsStatic: false } &&
		   (parameter.Constructor.Attribute<CandidateAttribute>()?.Enabled ?? true);
}
