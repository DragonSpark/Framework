namespace DragonSpark.Application.Navigation.Security;

public readonly record struct AuthenticateAddressInput(string Provider, string ReturnUrl);