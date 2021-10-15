using DragonSpark.Runtime.Execution;
using DragonSpark.Text;

namespace DragonSpark.Diagnostics.Logging.Text;

sealed class DetailsFormatter : IFormatter<Details>
{
	public static DetailsFormatter Default { get; } = new DetailsFormatter();

	DetailsFormatter() : this(TimestampFormat.Default) {}

	readonly string _format;

	public DetailsFormatter(string format) => _format = format;

	public string Get(Details parameter) => $"[{parameter.Observed.ToString(_format)}] {parameter.Name}";
}