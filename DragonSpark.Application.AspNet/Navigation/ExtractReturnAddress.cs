using DragonSpark.Application.AspNet.Security.Identity.Model;
using DragonSpark.Model.Selection;
using System.Linq;
using System.Web;

namespace DragonSpark.Application.AspNet.Navigation;

public sealed class ExtractReturnAddress : ISelect<string, string?>
{
	public static ExtractReturnAddress Default { get; } = new();

	ExtractReturnAddress() : this(ReturnUrl.Default, nameof(ReturnUrl)) {}

	readonly string _first, _second;

	public ExtractReturnAddress(string first, string second)
	{
		_first  = first;
		_second = second;
	}

	public string? Get(string parameter)
	{
		var query  = HttpUtility.ParseQueryString(parameter.Split('?').Last());
		var result = query.Get(_first) ?? query.Get(_second) ?? null;
		return result;
	}
}