using System;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static partial class StringExtensions
	{
		public static string Capitalized( this string target )
		{
			var result = string.IsNullOrEmpty( target ) ? string.Empty : char.ToUpper( target[ 0 ] ) + target.Substring( 1 );
			return result;
		}

		public static string NullIfEmpty( this string target )
		{
			var result = string.IsNullOrEmpty( target ) ? null : target;
			return result;
		}

		public static string[] ToStringArray( this string target )
		{
			return ToStringArray( target, ',', ';' );
		}

		public static string[] ToStringArray( this string target, params char[] delimiters )
		{
			var items =
				from item in ( target ?? string.Empty ).Split( delimiters, StringSplitOptions.RemoveEmptyEntries )
				select item.Trim();
			var result = items.ToArray();
			return result;
		}
	}
}