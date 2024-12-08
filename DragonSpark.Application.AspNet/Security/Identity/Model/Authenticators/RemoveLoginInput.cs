namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

public readonly record struct RemoveLoginInput<T>(T User, ProviderIdentity Identity);