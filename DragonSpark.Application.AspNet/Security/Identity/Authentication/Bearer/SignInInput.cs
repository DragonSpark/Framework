using DragonSpark.Application.Communication.Http.Security;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public readonly record struct SignInInput<T>(T User, ChallengeRequest Challenge);