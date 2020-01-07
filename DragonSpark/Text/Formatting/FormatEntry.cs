using DragonSpark.Model;
using System;

namespace DragonSpark.Text.Formatting
{
	public class FormatEntry<T> : Pairing<string, Func<T, string>>, IFormatEntry<T>
	{
		protected FormatEntry(string key, Func<T, string> value) : base(key, value) {}
	}
}