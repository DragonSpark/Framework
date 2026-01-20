namespace DragonSpark.Contracts.Security;
public sealed record AccessTokenResponse(
    string AccessToken,
    string RefreshToken,
    long ExpiresIn,
    string TokenType = "Bearer");