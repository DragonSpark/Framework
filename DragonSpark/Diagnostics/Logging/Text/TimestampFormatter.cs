using DragonSpark.Text.Formatting;

namespace DragonSpark.Diagnostics.Logging.Text;

public sealed class TimestampFormatter : DateTimeOffsetFormatter
{
	public static TimestampFormatter Default { get; } = new TimestampFormatter();

	TimestampFormatter() : base(TimestampFormat.Default) {}
}