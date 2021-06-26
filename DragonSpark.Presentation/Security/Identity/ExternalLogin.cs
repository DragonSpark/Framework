using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using System.Net;

namespace DragonSpark.Presentation.Security.Identity
{
	public class ExternalLogin : IAlteration<string>
	{
		readonly IResult<string> _return;
		readonly string          _path;

		public ExternalLogin(string @return) : this(@return.Start().Get()) {}

		public ExternalLogin(IResult<string> @return) : this(@return, ExternalLoginPath.Default) {}

		public ExternalLogin(IResult<string> @return, string path)
		{
			_return = @return;
			_path   = path;
		}

		public string Get(string parameter)
			=> $"{_path}?provider={parameter}&returnUrl={WebUtility.UrlEncode(_return.Get())}";
	}
}