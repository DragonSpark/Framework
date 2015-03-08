using System;
using System.IO;

namespace DragonSpark.Extensions
{
	public static class UriExtensions
	{
		static readonly string Slash = Path.AltDirectorySeparatorChar.ToString();

		public static Uri EnsureTrailingSlash( this Uri target )
		{
			var result = target.OriginalString.EndsWith( Slash ) ? target : new Uri( string.Concat( target.OriginalString, Slash ), target.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative );
			return result;
		}

		public static Uri Ensure( this Uri target, string scheme )
		{
			var result = target.Scheme != scheme ? new UriBuilder( target ) { Scheme = scheme, Port = -1 }.Uri : target;
			return result;
		}
	}
}