using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DragonSpark.Extensions
{
	public static class StringExtensions
	{
		public static string Capitalized( this string target ) => string.IsNullOrEmpty( target ) ? string.Empty : char.ToUpper( target[ 0 ] ) + target.Substring( 1 );

		public static string NullIfEmpty( this string target ) => string.IsNullOrEmpty( target ) ? null : target;

		public static string[] ToStringArray( this string target ) => ToStringArray( target, ',', ';' );

		public static string[] ToStringArray( this string target, params char[] delimiters )
		{
			var items =
				from item in ( target ?? string.Empty ).Split( delimiters, StringSplitOptions.RemoveEmptyEntries )
				select item.Trim();
			var result = items.ToArray();
			return result;
		}

		public static string TrimStartOf( this string @this, params char[] chars )
		{
			var result = chars.Select( c => c.ToString() ).Any( @this.StartsWith ) ? @this.Substring( 1 ) : @this;
			return result;
		}
	}

	// ATTRIBUTION: http://stackoverflow.com/questions/773303/splitting-camelcase
	public static class ConventionBasedFormattingExtensions
	{
		readonly static Regex SplitCamelCaseRegex = new Regex(@"
			(
				(?<=[a-z])[A-Z0-9] (?# lower-to-other boundaries )
				|
				(?<=[0-9])[a-zA-Z] (?# number-to-other boundaries )
				|
				(?<=[A-Z])[0-9] (?# cap-to-number boundaries; handles a specific issue with the next condition )
				|
				(?<=[A-Z])[A-Z](?=[a-z]) (?# handles longer strings of caps like ID or CMS by splitting off the last capital )
			)"
			, RegexOptions.IgnorePatternWhitespace
		);

		public static string[] SplitCamelCase( this string input )
		{
			var separated = SplitCamelCaseRegex.Replace( input, @" $1" ).Trim();
			var source = separated.Cast<char>().FirstOrDefault().With( c => char.IsLower( c ) ? char.ToUpper( c ) + separated.Substring( 1 ) : null ) ?? separated;
			var result = source.ToStringArray( ' ' );
			return result;
		}
	}
}