using DragonSpark.Model.Selection.Alterations;
using System.Net;

namespace DragonSpark.Application.Navigation;

public class TemplatedPath : IAlteration<string>
{
	readonly string _template;

	public TemplatedPath(string template) => _template = template;

	public string Get(string parameter)
	{
		var @return = WebUtility.UrlEncode(parameter);
		var result  = string.Format(_template, @return);
		return result;
	}
}