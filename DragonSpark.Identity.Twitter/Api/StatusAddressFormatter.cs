using DragonSpark.Application.Model.Text;

namespace DragonSpark.Identity.Twitter.Api;

public sealed class StatusAddressFormatter : Formatted<ulong>
{
	public static StatusAddressFormatter Default { get; } = new();

	StatusAddressFormatter() : base("https://twitter.com/anyuser/status/{0}/") {}
}