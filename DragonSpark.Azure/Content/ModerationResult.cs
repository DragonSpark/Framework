namespace DragonSpark.Azure.Content;

public record ModerationResult(bool Success, IReadOnlyCollection<string> Violations)
{
	public ModerationResult(IReadOnlyCollection<string> violations) : this(!violations.Any(), violations) {}
}