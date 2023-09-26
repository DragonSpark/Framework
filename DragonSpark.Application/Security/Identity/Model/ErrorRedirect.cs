using DragonSpark.Model;

namespace DragonSpark.Application.Security.Identity.Model;

public record ErrorRedirect(string Location, Pair<string, string> Message, string Origin)
{
	protected ErrorRedirect(string location, string message, string origin)
		: this(location, Pairs.Create("ErrorMessage", message), origin) {}
}