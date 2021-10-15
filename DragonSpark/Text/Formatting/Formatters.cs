using DragonSpark.Runtime.Invocation;
using System;

namespace DragonSpark.Text.Formatting;

sealed class Formatters<T> : Invocation0<T, ISelectFormatter<T>, IFormattable>
{
	public Formatters(ISelectFormatter<T> parameter)
		: base((instance, formatter) => new Adapter<T>(instance, formatter.Get), parameter) {}
}