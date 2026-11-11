using DragonSpark.Model.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Runtime;

[UsedImplicitly]
public sealed class TimePaths : ISelect<DateTimeOffset, string>
{
	public static TimePaths Default { get; } = new();

	TimePaths() : this("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture) {}

	readonly string      _format;
	readonly CultureInfo _culture;

	public TimePaths(string format, CultureInfo culture)
	{
		_format  = format;
		_culture = culture;
	}

	public string Get(DateTimeOffset parameter) => parameter.ToString(_format, _culture);
}