using DragonSpark.Diagnostics.Logging.Text;
using JetBrains.Annotations;

namespace DragonSpark.Application.Hosting.Console;

[UsedImplicitly]
sealed class ConsoleMessageTemplate : Text.Text
{
	public static ConsoleMessageTemplate Default { get; } = new();

	ConsoleMessageTemplate() : base($"{TemplateHeader.Default} {{Message:l}}{{NewLine}}{{Exception}}") {}
}