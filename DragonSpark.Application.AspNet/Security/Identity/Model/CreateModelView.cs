namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed record CreateModelView(ProviderIdentity Identity, string Name, string Address);