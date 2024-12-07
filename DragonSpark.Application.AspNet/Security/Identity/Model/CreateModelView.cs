namespace DragonSpark.Application.Security.Identity.Model;

public sealed record CreateModelView(ProviderIdentity Identity, string Name, string Address);