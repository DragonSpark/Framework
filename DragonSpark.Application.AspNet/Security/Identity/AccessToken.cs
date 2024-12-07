using DragonSpark.Text;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Application.Security.Identity;

public sealed class AccessToken : IFormatter<AuthenticationProperties>
{
	public static AccessToken Default { get; } = new();

	AccessToken() : this(".Token.access_token") {}

	readonly string _value;

	public AccessToken(string value) => _value = value;

	public string Get(AuthenticationProperties parameter) => parameter.GetString(_value) ?? string.Empty;
}