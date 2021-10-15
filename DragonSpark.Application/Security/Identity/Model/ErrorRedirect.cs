using DragonSpark.Model;

namespace DragonSpark.Application.Security.Identity.Model;

public class ErrorRedirect
{
	public ErrorRedirect(string location, string message, string origin)
		: this(location, Pairs.Create("ErrorMessage", message), origin) {}

	public ErrorRedirect(string location, Pair<string, string> message, string origin)
	{
		Location = location;
		Message  = message;
		Origin   = origin;
	}

	public string Location { get; }
	public Pair<string, string> Message { get; }
	public string Origin { get; }
}