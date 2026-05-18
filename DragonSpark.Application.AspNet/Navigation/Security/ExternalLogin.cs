using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public class ExternalLogin : IAlteration<string>
{
	readonly IResult<string>                      _return;
	readonly IFormatter<AuthenticateAddressInput> _path;

    protected ExternalLogin(string @return, AuthenticateAddress address) : this(@return.Start().Get(), address) {}

    protected ExternalLogin(IResult<string> @return, IFormatter<AuthenticateAddressInput> path)
	{
		_return = @return;
		_path   = path;
	}

	public string Get(string parameter) => _path.Get(new(parameter, _return.Get()));
}