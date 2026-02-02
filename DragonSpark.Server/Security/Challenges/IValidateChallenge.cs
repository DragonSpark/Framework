using DragonSpark.Contracts.Security;
using DragonSpark.Model.Selection;

namespace DragonSpark.Server.Security.Challenges;

public interface IValidateChallenge : ISelect<string, ChallengeTokenPayload>;