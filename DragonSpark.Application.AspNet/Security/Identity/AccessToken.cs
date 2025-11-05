using DragonSpark.Text;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class GetClientDetails : IFormatter<AuthenticationProperties>
{
	public static GetClientDetails Default { get; } = new();

	GetClientDetails() : this(".Token.access_token") {}

	readonly string _value;

	public GetClientDetails(string value) => _value = value;

	public string Get(AuthenticationProperties parameter) => parameter.GetString(_value) ?? string.Empty;
}