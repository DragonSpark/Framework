namespace DragonSpark.Diagnostics.Logging.Text;

public sealed class OutputTemplate : DragonSpark.Text.Text
{
	public static OutputTemplate Default { get; } = new OutputTemplate();

	OutputTemplate() : base($"{TemplateHeader.Default} ({{SourceContext}}) {{Message}}{{NewLine}}{{Exception}}") {}
}