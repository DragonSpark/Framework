using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Server.Security.Challenges;

sealed class ChallengeHasher : Alteration<string>, IChallengeHasher
{
    public ChallengeHasher(ChallengeSettings settings) : base(new HmacSha256Hasher(settings.Key)) {}
}