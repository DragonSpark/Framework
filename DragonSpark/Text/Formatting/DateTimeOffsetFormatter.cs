namespace DragonSpark.Text.Formatting;

public class DateTimeOffsetFormatter : IFormatter<DateTimeOffset>
{
	readonly string      _format;
	readonly CultureInfo _culture;

	protected DateTimeOffsetFormatter(string format) : this(format, CultureInfo.InvariantCulture) {}

	protected DateTimeOffsetFormatter(string format, CultureInfo culture)
	{
		_format       = format;
		_culture = culture;
	}

	public string Get(DateTimeOffset parameter) => parameter.ToString(_format, _culture);
}