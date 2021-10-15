using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text.Formatting;

class Adapter<T> : IFormattable
{
	readonly Func<string?, Func<T, string>?> _selector;
	readonly T                               _subject;

	public Adapter(T subject, ISelect<T, string> format) : this(subject, format.ToDelegate().Accept) {}

	public Adapter(T subject, Func<string?, Func<T, string>?> selector)
	{
		_subject  = subject;
		_selector = selector;
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> _selector(format)?.Invoke(_subject) ?? string.Empty;
}