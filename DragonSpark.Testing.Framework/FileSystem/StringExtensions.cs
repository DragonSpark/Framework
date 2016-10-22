using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	internal static class StringExtensions
	{
		[Pure]
		internal static string[] SplitLines(this string input)
		{
			var list = new List<string>();
			using (var reader = new StringReader(input))
			{
				string str;
				while ((str = reader.ReadLine()) != null)
				{
					list.Add(str);
				}
			}

			return list.ToArray();
		}

		[Pure]
		public static string Replace(this string source, string oldValue, [Optional]string newValue, StringComparison comparisonType)
		{
			// from http://stackoverflow.com/a/22565605 with some adaptions
			if (string.IsNullOrEmpty(oldValue))
			{
				throw new ArgumentNullException(nameof( oldValue ));
			}

			if (source.Length == 0)
			{
				return source;
			}

			var replace = newValue ?? string.Empty;
			var result = new StringBuilder();
			int startingPos = 0;
			int nextMatch;
			while ((nextMatch = source.IndexOf(oldValue, startingPos, comparisonType)) > -1)
			{
				result.Append(source, startingPos, nextMatch - startingPos);
				result.Append(replace);
				startingPos = nextMatch + oldValue.Length;
			}

			result.Append(source, startingPos, source.Length - startingPos);

			return result.ToString();
		}
	}
}