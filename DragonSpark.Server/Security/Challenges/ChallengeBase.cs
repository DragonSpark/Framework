using System.Security;
using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Server.Security.Challenges;

public class ChallengeBase : Result<ChallengeResponse>
{
    protected ChallengeBase(INewChallenge @new, string purpose) : base(@new.Then().Bind(purpose)) {}
}

// TODO

public class ParseChallengeBase : IParser<string?>
{
    readonly IValidateChallenge _validate;
    readonly string             _purpose;

    protected ParseChallengeBase(IValidateChallenge validate, string purpose)
    {
        _validate     = validate;
        _purpose = purpose;
    }

    public string? Get(string parameter)
    {
        try
        {
            var payload = _validate.Get(parameter);
            return payload.Purpose == _purpose ? payload.Challenge : null;
        }
        catch (SecurityException)
        {
            return null;
        }
    }
}