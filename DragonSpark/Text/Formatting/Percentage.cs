namespace DragonSpark.Text.Formatting;

public sealed class Percentage : IFormatter<float>
{
	public static Percentage Default { get; } = new();

	Percentage() : this("P2") {}

	readonly string      _template;
	readonly string      _default;
	readonly CultureInfo _culture;

	public Percentage(string template) : this(template, 0.ToString(template), CultureInfo.InvariantCulture) {}

	public Percentage(string template, string @default, CultureInfo culture)
	{
		_template = template;
		_default  = @default;
		_culture  = culture;
	}

	public string Get(float parameter) => float.IsNaN(parameter) ? _default : parameter.ToString(_template, _culture);
}