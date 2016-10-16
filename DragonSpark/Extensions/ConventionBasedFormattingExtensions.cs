using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace DragonSpark.Extensions
{
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

		public static ImmutableArray<string> SplitCamelCase( this string input )
		{
			var separated = SplitCamelCaseRegex.Replace( input, @" $1" ).Trim();
			var source = separated.Cast<char>().FirstOrDefault().With( c => char.IsLower( c ) ? $"{char.ToUpper( c ).ToString()}{separated.Substring( 1 )}" : null ) ?? separated;
			var result = source.ToStringArray( ' ' );
			return result;
		}
	}
}