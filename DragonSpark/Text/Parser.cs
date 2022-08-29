using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text;

public class Parser<T> : Select<string, T>, IParser<T>
{
	protected Parser(ISelect<string, T> @select) : base(@select) {}

	protected Parser(Func<string, T> @select) : base(@select) {}
}