namespace DragonSpark.Diagnostics.Logging.Text;

public sealed class TemplateHeader : DragonSpark.Text.Text
{
	public static TemplateHeader Default { get; } = new TemplateHeader();

	TemplateHeader() : base($"[{{Timestamp:{TimestampFormat.Default}}} {{Level:u3}}]") {}
}