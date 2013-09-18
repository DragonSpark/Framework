using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DragonSpark.Extensions
{
	public class StringReplacementDictionary : Dictionary<string, string>
	{}

	public static class StringExtensions
	{
		public static string GenerateSlug( this string phrase )
		{
			var result = Regex.Replace( phrase.RemoveAccent().ToLower(), @"[^a-zA-Z0-9]", "-" ).Transform( x => x.Substring( 0, x.Length ) );
			// invalid chars           
			/*// convert multiple spaces into one space   
			str = Regex.Replace( str, @"\s+", " " ).Trim();
			// cut and trim 
			str = str.Substring( 0, str.Length <= 45 ? str.Length : 45 ).Trim();
			str = Regex.Replace( str, @"\s", "-" ); // hyphens   */

			return result;
		}

		public static string RemoveAccent( this string txt )
		{
			var bytes = Encoding.GetEncoding( "Cyrillic" ).GetBytes( txt );
			return Encoding.ASCII.GetString( bytes );
		}


		/*public static string ToMembershipName( this string target )
		{
			NetworkCredential credential = CredentialHelper.Create( target, null );
			string result = credential.UserName ?? string.Empty;
			return result;
		}*/

		public static string WithReplacements( this string target, StringReplacementDictionary replacements = null )
		{
			var parameter = replacements ?? ServiceLocation.Locate<StringReplacementDictionary>();
			var result = parameter.Transform( x => string.Format( NamedTokenFormatter.Instance, target, parameter ) ) ?? target;
			return result;
		}

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