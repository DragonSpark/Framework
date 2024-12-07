using DragonSpark.Model.Selection.Alterations;
using System.Net;

namespace DragonSpark.Application.Navigation;

public class ReturnPath : IAlteration<string>
{
	readonly string _path;

	protected ReturnPath(string path) => _path = path;

	public string Get(string parameter) => $"{_path}?returnUrl={WebUtility.UrlEncode(parameter)}";
}