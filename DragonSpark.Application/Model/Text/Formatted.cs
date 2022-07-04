using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Model.Text;

public class Formatted<T> : Formatter<T> where T : notnull
{
	protected Formatted(string template) : base(template.Start().Get().Format) {}
}