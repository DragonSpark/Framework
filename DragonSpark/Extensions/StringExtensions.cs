using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace DragonSpark.Extensions
{
	public static class StringExtensions
	{
		public static string Capitalized( this string target ) => string.IsNullOrEmpty( target ) ? string.Empty : $"{char.ToUpper( target[0] )}{target.Substring( 1 )}";

		public static string NullIfEmpty( [Optional]this string target ) => string.IsNullOrEmpty( target ) ? null : target;

		public static string[] ToStringArray( this string target ) => ToStringArray( target, ',', ';' );

		public static string[] ToStringArray( this string target, params char[] delimiters )
		{
			var items =
				from item in ( target ?? string.Empty ).Split( delimiters, StringSplitOptions.RemoveEmptyEntries )
				select item.Trim();
			var result = items.ToArray();
			return result;
		}

		public static string TrimStartOf( this string @this, params char[] characters )
		{
			foreach ( var c in characters )
			{
				if ( @this.StartsWith( c.ToString() ) )
				{
					return @this.Substring( 1 );
				}
			}

			return @this;
		}
	}
}