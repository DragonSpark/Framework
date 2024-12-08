using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public class ExternalLogin : IAlteration<string>
{
	readonly IResult<string>                      _return;
	readonly IFormatter<AuthenticateAddressInput> _path;

	public ExternalLogin(string @return) : this(@return.Start().Get()) {}

	public ExternalLogin(IResult<string> @return) : this(@return, AuthenticateAddress.Default) {}

	public ExternalLogin(IResult<string> @return, IFormatter<AuthenticateAddressInput> path)
	{
		_return = @return;
		_path   = path;
	}

	public string Get(string parameter) => _path.Get(new(parameter, _return.Get()));
}