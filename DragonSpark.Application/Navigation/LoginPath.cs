using DragonSpark.Model.Selection.Alterations;
using System.Net;

namespace DragonSpark.Application.Navigation
{
	public sealed class LoginPath : IAlteration<string>
	{
		public static LoginPath Default { get; } = new LoginPath();

		LoginPath() : this(LoginPathTemplate.Default) {}

		readonly string _template;

		public LoginPath(string template) => _template = template;

		public string Get(string parameter)
		{
			var @return = WebUtility.UrlEncode(parameter);
			var result  = string.Format(_template, @return);
			return result;
		}
	}
}
