namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public readonly record struct SignInInput<T>(T User, string Subject);