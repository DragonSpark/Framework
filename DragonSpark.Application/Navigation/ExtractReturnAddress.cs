using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Text;
using System.Linq;
using System.Web;

namespace DragonSpark.Application.Navigation;

public sealed class ExtractReturnAddress : IFormatter<string>
{
	public static ExtractReturnAddress Default { get; } = new();

	ExtractReturnAddress() : this(ReturnUrl.Default, nameof(ReturnUrl)) {}

	readonly string _first, _second;

	public ExtractReturnAddress(string first, string second)
	{
		_first  = first;
		_second = second;
	}

	public string Get(string parameter)
	{
		var query  = HttpUtility.ParseQueryString(parameter.Split('?').Last());
		var result = query.Get(_first) ?? query.Get(_second) ?? parameter;
		return result;
	}
}