namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class ChallengeStateKey : Text.Text
{
	public static ChallengeStateKey Default { get; } = new();

	ChallengeStateKey() : base("dsreqchllge") {}
}