using DragonSpark.Model.Selection.Alterations;
using SmartFormat;

namespace DragonSpark.Application.Model.Text;

public class SmartFormat : IAlteration<string>
{
	readonly string _template;

	public SmartFormat(string template) => _template = template;

	public string Get(string parameter) => _template.FormatSmart(parameter);
}