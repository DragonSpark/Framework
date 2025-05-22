namespace DragonSpark.Application.Communication.Http.Security;
public sealed record AccessTokenResponse(
    string AccessToken,
    string RefreshToken,
    long ExpiresIn,
    string TokenType = "Bearer");