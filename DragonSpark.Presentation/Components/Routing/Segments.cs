using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Presentation.Components.Routing
{
	sealed class Segments : IArray<string, string>
	{
		public static Segments Default { get; } = new Segments();

		Segments() : this(Uri.UnescapeDataString, new[] {'/'}) {}

		readonly Func<string, string> _selector;
		readonly char[]               _separator;

		public Segments(Func<string, string> selector, char[] separator)
		{
			_selector  = selector;
			_separator = separator;
		}

		public Array<string> Get(string parameter)
		{
			var result = parameter.Trim('/').Split(_separator, StringSplitOptions.RemoveEmptyEntries);
			var length = result.Length;
			for (var i = 0; i < length; i++)
			{
				result[i] = _selector(result[i]);
			}

			return result
				;
		}
	}
}