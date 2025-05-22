namespace DragonSpark.Application.Communication.Http.Security;

public readonly record struct PersistAccessTokenViewInput(string Identifier, AccessTokenResponse Response);