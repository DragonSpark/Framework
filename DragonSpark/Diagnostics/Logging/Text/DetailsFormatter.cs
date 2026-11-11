using DragonSpark.Runtime.Execution;
using DragonSpark.Text;

namespace DragonSpark.Diagnostics.Logging.Text;

sealed class DetailsFormatter : IFormatter<Details>
{
	public static DetailsFormatter Default { get; } = new();

	DetailsFormatter() : this(TimestampFormat.Default, CultureInfo.InvariantCulture) {}

	readonly string      _format;
	readonly CultureInfo _culture;

	public DetailsFormatter(string format, CultureInfo culture)
	{
		_format  = format;
		_culture = culture;
	}

	public string Get(Details parameter) => $"[{parameter.Observed.ToString(_format, _culture)}] {parameter.Name}";
}