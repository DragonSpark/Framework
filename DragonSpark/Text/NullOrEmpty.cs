using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public sealed class NullOrEmpty : ISelect<string?, string>
{
	public static NullOrEmpty Default { get; } = new NullOrEmpty();

	NullOrEmpty() {}

	public string Get(string? parameter) => parameter ?? string.Empty;
}