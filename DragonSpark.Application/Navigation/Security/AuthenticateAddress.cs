using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using Humanizer;

namespace DragonSpark.Application.Navigation.Security;

public sealed class AuthenticateAddress : IFormatter<AuthenticateAddressInput>
{
	public static AuthenticateAddress Default { get; } = new();

	AuthenticateAddress()
		: this($"{ExternalLoginPath.Default.Get().ToLower()}?provider={{0}}&returnUrl={{1}}", UrlEncode.Default) {}

	readonly string              _template;
	readonly IAlteration<string> _encode;

	public AuthenticateAddress(string template, IAlteration<string> encode)
	{
		_template = template;
		_encode   = encode;
	}

	public string Get(AuthenticateAddressInput parameter)
	{
		var (provider, returnUrl) = parameter;
		return _template.FormatWith(provider, _encode.Get(returnUrl));
	}
}