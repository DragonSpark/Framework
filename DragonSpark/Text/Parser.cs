using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public class Parser<T> : Select<string, T>, IParser<T>
{
    public Parser(ISelect<string, T> @select) : base(@select) {}

    public Parser(Func<string, T> @select) : base(@select) {}
}