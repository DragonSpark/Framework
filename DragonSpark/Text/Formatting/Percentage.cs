namespace DragonSpark.Text.Formatting;

public sealed class Percentage : IFormatter<float>
{
	public static Percentage Default { get; } = new();

	Percentage() : this("P2") {}

	readonly string _template;
	readonly string _default;

	public Percentage(string template) : this(template, 0.ToString(template)) {}

	public Percentage(string template, string @default)
	{
		_template = template;
		_default  = @default;
	}

	public string Get(float parameter) => float.IsNaN(parameter) ? _default : parameter.ToString(_template);
}