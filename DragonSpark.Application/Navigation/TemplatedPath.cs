using DragonSpark.Model.Selection.Alterations;
using System.Net;

namespace DragonSpark.Application.Navigation;

public class TemplatedPath(string template) : IAlteration<string>
{
	readonly string _template = template;

	public string Get(string parameter)
	{
		var @return = WebUtility.UrlEncode(parameter);
		var result  = string.Format(_template, @return);
		return result;
	}
}