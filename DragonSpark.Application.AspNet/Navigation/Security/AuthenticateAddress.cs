using DragonSpark.Application.Navigation;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class AuthenticateAddress : IFormatter<AuthenticateAddressInput>
{
	readonly string              _template;
	readonly IAlteration<string> _encode;

	public AuthenticateAddress(ExternalLoginPath path) : this(path.Get()) {}

	public AuthenticateAddress(string path) : this($"{path}?provider={{0}}&returnUrl={{1}}", UrlEncode.Default) {}
	
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