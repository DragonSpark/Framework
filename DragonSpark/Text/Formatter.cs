using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public class Formatter<T> : Select<T, string>, IFormatter<T>
{
	protected Formatter(ISelect<T, string> @select) : base(@select) {}

	protected Formatter(Func<T, string> @select) : base(@select) {}
}