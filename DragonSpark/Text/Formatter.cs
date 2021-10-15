using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text;

public class Formatter<T> : Select<T, string>, IFormatter<T>
{
	public Formatter(ISelect<T, string> @select) : base(@select) {}

	public Formatter(Func<T, string> @select) : base(@select) {}
}