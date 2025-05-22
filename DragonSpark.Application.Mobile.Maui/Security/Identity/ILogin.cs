using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

public interface ILogin : IStopAwareDepending<ChallengeRequest>;