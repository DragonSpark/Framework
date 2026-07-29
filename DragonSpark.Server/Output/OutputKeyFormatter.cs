using DragonSpark.Text;

namespace DragonSpark.Server.Output;

public sealed class OutputKeyFormatter : Formatter<OutputKeyFormatterInput>
{
	public static OutputKeyFormatter Default { get; } = new();

	OutputKeyFormatter() : base(x => $"{x.Key}:{x.Input}") {}
}