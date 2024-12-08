namespace DragonSpark.Application.AspNet.Navigation.Security;

public readonly record struct AuthenticateAddressInput(string Provider, string ReturnUrl);