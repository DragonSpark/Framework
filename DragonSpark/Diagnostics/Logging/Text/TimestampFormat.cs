namespace DragonSpark.Diagnostics.Logging.Text;

public sealed class TimestampFormat : DragonSpark.Text.Text
{
	public static TimestampFormat Default { get; } = new TimestampFormat();

	TimestampFormat() : base("HH:mm:ss:fff") {}
}