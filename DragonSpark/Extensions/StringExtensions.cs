using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Extensions
{
	public class StringReplacementDictionary : Dictionary<string, string>
	{}

	public static class StringExtensions
	{
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