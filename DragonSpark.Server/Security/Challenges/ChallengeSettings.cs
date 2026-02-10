namespace DragonSpark.Server.Security.Challenges;

public sealed record ChallengeSettings
{
    public required string Key { get; init; }
}