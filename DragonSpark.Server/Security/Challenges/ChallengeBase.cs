using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Results;

namespace DragonSpark.Server.Security.Challenges;

public class ChallengeBase : Result<ChallengeResponse>
{
    protected ChallengeBase(INewChallenge @new, string purpose) : base(@new.Then().Bind(purpose)) {}
}